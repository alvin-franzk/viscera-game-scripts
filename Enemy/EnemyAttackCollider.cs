using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackCollider : MonoBehaviour
{
    private bool isCollidingWithPlayer;

    public bool IsCollidingWithPlayer
    {
        get { return isCollidingWithPlayer; }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.name == "Player Viscera")
        {
            isCollidingWithPlayer = true;
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.name == "Player Viscera")
        {
            isCollidingWithPlayer = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.name == "Player Viscera")
        {
            // Player just left attack range
            isCollidingWithPlayer = false;
        }    
    }
}
