using UnityEngine;

public class TPPCameraController : MonoBehaviour
{
    [Tooltip("Drag your player GameObject here")]
    public GameObject player;

    [Tooltip("How much the camera follows the player’s X movement (0 = no follow, 1 = exact follow)")]
    [Range(0f, 1f)]
    public float xFollowFactor = 0.4f; 

    // We cache the initial positions so we have a stable reference:
    private float initialCameraX;
    private float initialPlayerX;
    private float offsetZ;
    private float fixedY;

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("TPPCameraController: Player reference is missing!");
            enabled = false;
            return;
        }

        // Record the camera’s and player’s initial X positions
        initialCameraX = transform.position.x;
        initialPlayerX = player.transform.position.x;

        // Record the camera’s initial Y (it remains constant)
        fixedY = transform.position.y;

        // Record how far ahead/behind on Z the camera starts relative to the player
        offsetZ = transform.position.z - player.transform.position.z;
    }

    void LateUpdate()
    {
        // 1) Compute new Z so the camera stays offset in front/behind
        float newZ = player.transform.position.z + offsetZ;

        // 2) Compute how far the player has moved from their initial X
        float deltaPlayerX = player.transform.position.x - initialPlayerX;

        // 3) Apply a reduced (damped) version of that delta to the camera’s X:
        float newX = initialCameraX + deltaPlayerX * xFollowFactor;

        // 4) Y remains fixed:
        float newY = fixedY;

        // 5) Update camera position
        transform.position = new Vector3(newX, newY, newZ);
    }
}

