using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager_Ground : MonoBehaviour {


    private PlayerMovement player_movement;

    private void Start()
    {
        player_movement = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.name == "Player")
        {
            player_movement.num_jumps = player_movement.max_jumps;
        }
    }
}
