using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    SwerveMovement playerMovement;

    void Start()
    {
        playerMovement = GameObject.FindObjectOfType<SwerveMovement>();

    }

    void OnCollisionEnter (Collision collision)
    {
        //killing player
        if (collision.gameObject.name == "Player")
        {
            playerMovement.Die();
        }
    }
}
