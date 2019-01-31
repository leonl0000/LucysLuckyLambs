using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public Rigidbody playerRB;
    public Camera cam;
    public float camSpeed;
    public float moveSpeed;

    private bool leftMove;
    private bool rightMove;
    private bool forwardMove;
    private bool backMove;
    private bool jump;
    private float angle;
    private Vector3 offsetAngle;

    void Start()
    {
        playerRB.AddForce(0, 200, 0);
    }

    private void Update()
    {
        //Rotates the player's facing direction based on Mouse X axis movement.
        angle += camSpeed * Input.GetAxis("Mouse X");
        offsetAngle = new Vector3(0, angle, 0);
        transform.eulerAngles = offsetAngle;

        leftMove = Input.GetKey("a");
        rightMove = Input.GetKey("d");
        forwardMove = Input.GetKey("w");
        backMove = Input.GetKey("s");
        jump = Input.GetKey(KeyCode.Space);
    }


    // Update is called once per frame
    void FixedUpdate() 
    {
        //Example from on Unity Website https://docs.unity3d.com/ScriptReference/Transform-forward.html
        if (forwardMove)
        {
            //Move the Rigidbody forwards constantly at speed you define (the blue arrow axis in Scene view)
            playerRB.velocity += transform.forward * moveSpeed;
        }

        if (backMove)
        {
            //Move the Rigidbody backwards constantly at the speed you define (the blue arrow axis in Scene view)
            playerRB.velocity += -transform.forward * moveSpeed;
        }

        if (rightMove)
        {
            //Rotate the sprite about the Y axis in the positive direction
            playerRB.velocity += transform.right * moveSpeed;
        }

        if (leftMove)
        {
            //Rotate the sprite about the Y axis in the negative direction
            playerRB.velocity += -transform.right * moveSpeed;
        }

        if (jump && transform.position.y == 1) playerRB.AddForce(0, 200, 0);

    }
}
