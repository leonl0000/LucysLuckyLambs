using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform playerTransform;
    public Vector3 offset;
    public float followDistance;


    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = playerTransform.position + (playerTransform.forward * -followDistance) + offset;
        transform.LookAt(playerTransform, Vector3.up);
    }


}
