using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{

    public Rigidbody playerRB;
    public Camera cam;
    public float camSpeed;
    public int max_velocity;
    public float moveSpeed;
    public int max_jumps;
    public int num_jumps;

    private Abilities abilities;
    private bool leftMove;
    private bool rightMove;
    private bool forwardMove;
    private bool backMove;
    private bool ab1;
    private bool jump;
    private float angle;
    private Vector3 offsetAngle;


    void Start()
    {
        playerRB.AddForce(0, 200, 0);
        abilities = this.gameObject.GetComponent<Abilities>();
        num_jumps = max_jumps;
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
        jump = Input.GetKeyDown(KeyCode.Space);
        ab1 = Input.GetKeyDown(KeyCode.Alpha1);
    }


    // Update is called once per frame
    void FixedUpdate() 
    {
        Vector3 delta_velocity = new Vector3(0, 0, 0);

        //Example from on Unity Website https://docs.unity3d.com/ScriptReference/Transform-forward.html
        if (forwardMove)
        {
            //Move the Rigidbody forwards constantly at speed you define (the blue arrow axis in Scene view)
            delta_velocity += transform.forward * moveSpeed;
        }

        if (backMove)
        {
            //Move the Rigidbody backwards constantly at the speed you define (the blue arrow axis in Scene view)
            delta_velocity += -transform.forward * moveSpeed;
        }

        if (rightMove)
        {
            //Rotate the sprite about the Y axis in the positive direction
            delta_velocity += transform.right * moveSpeed;
        }

        if (leftMove)
        {
            //Rotate the sprite about the Y axis in the negative direction
            delta_velocity += -transform.right * moveSpeed;
        }

        if (Mathf.Abs((playerRB.velocity + delta_velocity).x) > max_velocity) delta_velocity.x = playerRB.velocity.x > 0 ? max_velocity - playerRB.velocity.x : -max_velocity - playerRB.velocity.x;
        if (Mathf.Abs((playerRB.velocity + delta_velocity).z) > max_velocity) delta_velocity.z = playerRB.velocity.z > 0 ? max_velocity - playerRB.velocity.z : -max_velocity - playerRB.velocity.z;

        playerRB.velocity += delta_velocity;

        if (jump && num_jumps > 0)
        {
            num_jumps--;
            playerRB.AddForce(0, 400, 0);
        }

        if (ab1)
        {
            abilities.SpawnLure();
        }

    }
}
