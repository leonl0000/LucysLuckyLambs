using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform playerTransform;
    public Vector3 offset;


    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = playerTransform.position + (playerTransform.forward * -7) + offset;
        transform.LookAt(playerTransform, Vector3.up);
    }


}
