using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SwerveMovement : MonoBehaviour
{
    
    [SerializeField] private float maxDisplacement = 0.2f;
    [SerializeField] private float maxPositionX = 2f;
    [SerializeField] LayerMask groundMask;  
    [SerializeField] Rigidbody rb;
    private Vector2 _anchorPosition;
    public float speed = 5; 
    bool alive = true;
    public float speedIncperpoint = 0.01f;

    private void Update()
    {
        var inputX = GetInput();
        var displacementX = GetDisplacement(inputX);
        var newPosition = GetNewLocalPosition(displacementX);
        newPosition = GetLimitedLocalPosition(newPosition);

        transform.localPosition = newPosition;
    }

    private Vector3 GetLimitedLocalPosition(Vector3 position)
    {
        position.x = Mathf.Clamp(position.x, -maxPositionX, maxPositionX);
        return position;
    }

    private Vector3 GetNewLocalPosition(float displacementX)
    {
        var lastPosition = transform.localPosition;
        var newPositionX = lastPosition.x + displacementX;
        var newPosition = new Vector3(newPositionX, lastPosition.y, lastPosition.z);
        return newPosition;
    }

    private float GetInput()
    {
        var inputX = 0f;
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                _anchorPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                inputX = (touch.position.x - _anchorPosition.x) / Screen.width;
                _anchorPosition = touch.position;
            }
        }
        return inputX;
    }

    private float GetDisplacement(float inputX)
    {
        var displacementX = 0f;
        displacementX = inputX * maxDisplacement;
        return displacementX;
    }

    void FixedUpdate()
    {
        if (!alive) return;
        transform.Translate(Vector3.forward * Time.fixedDeltaTime * speed);
    }
    
    public void Die()
    {
        //Restart the game
        alive = false;
        Invoke("Restart" , 2);
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //restarting game
    }
}