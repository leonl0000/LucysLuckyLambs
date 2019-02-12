using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform playerTransform;
    public Vector3 offset;
    public int positionBack; // how far behind the player the camera is positioned
    public int lookForward; // how far in front of the player the camera is looking


    // Update is called once per frame
    void Update()
    {
        transform.position = playerTransform.position + (playerTransform.forward * -positionBack) + offset;
        transform.LookAt(playerTransform.position + playerTransform.forward * lookForward, Vector3.up);
    }

}
