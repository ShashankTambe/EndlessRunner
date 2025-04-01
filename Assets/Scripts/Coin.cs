using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] float CoinTurnSpeed = 90f; 

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Obstacle>() != null)
        {
            Destroy(gameObject);
            return;
        }
        //check if object is player
        if(other.gameObject.name != "Player")
        {
            return;
        }
        //player score
        GameManager.inst.IncrementScore();
        //destroy coin
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0,0,CoinTurnSpeed * Time.deltaTime);
    }
}
