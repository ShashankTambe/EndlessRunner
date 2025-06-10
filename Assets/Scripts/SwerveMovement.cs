using UnityEngine;
using UnityEngine.InputSystem; // Essential for the new Input System

public class SwerveMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] public float speed = 5f;
    [SerializeField] private float swerveSensitivity = 0.01f; // How much deltaX translates to X movement
    [SerializeField] private float maxPositionX = 2f;
    public float speedIncPerPoint = 0.01f;   // Overwritten by PlayerPrefs in GameManager

    [Header("Jump Settings")]
    [SerializeField] private float jumpVelocity = 5f;         // Sets upward velocity on jump
    [SerializeField] private float fallMultiplier = 2.5f;     // Multiplier for stronger gravity when falling
    [SerializeField] private LayerMask groundLayer;           // Which layers count as “ground”
    [SerializeField] private float groundCheckDistance = 0.2f; // Ray length to confirm grounded
    [SerializeField] private float jumpDeltaThreshold = 0.02f; // Minimum deltaY in one frame to trigger jump

    private Rigidbody rb;
    private PlayerControls playerControls;              // Generated InputActions asset
    private PlayerControls.PlayerActions playerActions; // “Player” action map struct
    private bool alive = true;                          // When false, stops input & movement

    // For resetting on retry
    private Vector3 initialPosition;
    private float initialSpeed;

    private void Awake()
    {
        // 1) Set up Input Actions
        playerControls = new PlayerControls();
        playerActions = playerControls.Player;

        // 2) Cache Rigidbody and check
        rb = GetComponent<Rigidbody>();
        if (rb == null)
            Debug.LogError("SwerveMovement requires a Rigidbody on the same GameObject.");

        // 3) Record initial state for ResetToStart
        initialPosition = transform.position;
        initialSpeed = speed;
    }

    private void OnEnable()
    {
        playerActions.Enable();
        playerActions.SwerveMovement.performed += OnSwervePerformed;
    }

    private void OnDisable()
    {
        playerActions.SwerveMovement.performed -= OnSwervePerformed;
        playerActions.Disable();
    }

    private void Start()
    {
        // Load speed increment from PlayerPrefs (set by difficulty)
        speedIncPerPoint = PlayerPrefs.GetFloat("SpeedIncPerCoin", speedIncPerPoint);
    }

    /// <summary>
    /// Called on each frame’s pointer‐delta event. Uses delta.x for swerve
    /// and checks delta.y to launch a jump.
    /// </summary>
    private void OnSwervePerformed(InputAction.CallbackContext context)
    {
        if (!alive) return;

        Vector2 deltaInput = context.ReadValue<Vector2>();
        float dx = deltaInput.x;
        float dy = deltaInput.y;

        // 1) Perform jump by setting vertical velocity directly if upward swipe is big enough
        if (dy > jumpDeltaThreshold && IsGrounded())
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpVelocity, rb.linearVelocity.z);
        }

        // 2) Swerve horizontally using deltaX
        float displacementX = dx * swerveSensitivity;
        ApplySwerveMovement(displacementX);
    }

    /// <summary>
    /// Constrain sideways movement to [-maxPositionX, maxPositionX].
    /// </summary>
    private void ApplySwerveMovement(float displacementX)
    {
        Vector3 lastPos = transform.localPosition;
        float newX = Mathf.Clamp(lastPos.x + displacementX, -maxPositionX, maxPositionX);
        transform.localPosition = new Vector3(newX, lastPos.y, lastPos.z);
    }

    private void FixedUpdate()
    {
        if (!alive) return;

        // 1) Forward movement
        transform.Translate(Vector3.forward * Time.fixedDeltaTime * speed);

        // 2) If we are above ground (rb.velocity.y ≠ 0), apply extra gravity
        if (!IsGrounded())
        {
            // When rising or falling, add additional downward acceleration:
            // Physics.gravity.y is negative; we multiply by (fallMultiplier - 1)
            Vector3 extraGravity = Vector3.up * Physics.gravity.y * (fallMultiplier - 1);
            rb.AddForce(extraGravity, ForceMode.Acceleration);
        }
    }

    /// <summary>
    /// Called by GameManager.IncrementScore() when a coin is collected.
    /// </summary>
    public void IncreaseSpeed(float amount)
    {
        speed += amount;
    }

    /// <summary>
    /// Called when the player “dies.” Disables input and notifies GameManager.
    /// </summary>
    public void Die()
    {
        alive = false;
        playerActions.Disable();
        if (GameManager.inst != null) GameManager.inst.OnPlayerDeath();
    }

    /// <summary>
    /// Resets the player to starting position, speed, and re-enables input.
    /// </summary>
    public void ResetToStart()
    {
        transform.position = initialPosition;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        speed = initialSpeed;
        alive = true;
        enabled = true;
        playerActions.Enable();
    }

    /// <summary>
    /// Simple raycast down to check if the player is on ground.
    /// </summary>
    private bool IsGrounded()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        return Physics.Raycast(ray, groundCheckDistance, groundLayer);
    }
}
