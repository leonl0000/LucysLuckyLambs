using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager_Ground : MonoBehaviour {


    private PlayerMovement player_movement;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.name == "Player")
        {
            player_movement = collision.collider.GetComponent<PlayerMovement>();
            player_movement.num_jumps = 3;
        }
    }
}
