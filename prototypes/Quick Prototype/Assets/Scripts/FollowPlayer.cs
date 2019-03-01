using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    public Vector3 offset;
    public int positionBack; // how far behind the player the camera is positioned
    public int lookForward; // how far in front of the player the camera is looking


    void FixedUpdate()
    {
        Transform cameraTransform = player.GetComponent<PlayerMovement>().cameraTransform;
        transform.position = player.transform.position + (cameraTransform.forward * -positionBack) + offset;
        transform.LookAt(player.transform.position + cameraTransform.forward * lookForward, Vector3.up);
    }

}
