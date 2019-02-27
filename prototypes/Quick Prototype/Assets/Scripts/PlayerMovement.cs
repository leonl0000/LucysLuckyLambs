using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants
{
    public enum PanType {MOUSE, KEY};
}


public class PlayerMovement : MonoBehaviour
{

    public Rigidbody playerRB;
    public Camera cam;
    public float camSpeed;
    public float max_velocity;
    public float moveSpeed;
    public int max_jumps;
    public int num_jumps;

    private Abilities abilities;
    private bool leftMove;
    private bool rightMove;
    private bool forwardMove;
    private bool backMove;
    private bool panRight;
    private bool panLeft;
    private bool ab1;
    private bool ab2;
    private bool jump;
    private float xangle;
    private float yangle;
    private bool panKey;
    public Constants.PanType pan_type;
    private Vector3 offsetAngle;
    

    void Start()
    {
        pan_type = Constants.PanType.MOUSE;
        playerRB.AddForce(0, 200, 0);
        abilities = this.gameObject.GetComponent<Abilities>();
        num_jumps = max_jumps;
    }

    private void Update()
    {
        panLeft = Input.GetKey("q");
        panRight = Input.GetKey("e");
        if(Input.GetKeyDown("p")) pan_type = pan_type == Constants.PanType.MOUSE ? Constants.PanType.KEY : Constants.PanType.MOUSE;
        leftMove = Input.GetKey("a");
        rightMove = Input.GetKey("d");
        forwardMove = Input.GetKey("w");
        backMove = Input.GetKey("s");
        jump = Input.GetKeyDown(KeyCode.Space);
        ab1 = Input.GetKeyDown(KeyCode.Alpha1);
        ab2 = Input.GetKeyDown(KeyCode.Alpha2);

        //Rotates the player's facing direction based on Mouse X and Y axis movement.
        if (pan_type == Constants.PanType.MOUSE)
        {
            xangle += camSpeed * Input.GetAxis("Mouse X");
            yangle += camSpeed * Input.GetAxis("Mouse Y");
            offsetAngle = new Vector3(yangle, xangle, 0);
            transform.eulerAngles = offsetAngle;
        } else
        {
            float axis = 0; ;
            if (panRight) axis = 1;
            if (panLeft) axis = -1;
            xangle += camSpeed * axis;
            offsetAngle = new Vector3(0, xangle, 0);
            transform.eulerAngles = offsetAngle;
            //if (panRight) transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 90, 0), camSpeed * Time.deltaTime);
            //else if (panLeft) transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, -90, 0), camSpeed * Time.deltaTime);
        }


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

        //If the velocity we're about to add will make us faster than max_velocity
        if (Mathf.Abs(Vector3.Magnitude(playerRB.velocity + delta_velocity)) > max_velocity) 
        {
            //Correct the delta_velocity to add exactly the remainder from playerRB.velocity to max_velcoity
            delta_velocity = ((playerRB.velocity + delta_velocity) / (Vector3.Magnitude(playerRB.velocity + delta_velocity) / max_velocity)) - playerRB.velocity;
        }
        
        playerRB.velocity += delta_velocity;

        //if (panKey) pan_type = pan_type == Constants.PanType.MOUSE ? Constants.PanType.KEY : Constants.PanType.MOUSE;


        if (jump && num_jumps > 0)
        {
            num_jumps--;
            playerRB.AddForce(0, 400, 0);
        }

        if (ab1)
        {
            abilities.SpawnLure();
        }
        if (ab2)
        {
            abilities.SpawnFireball();
        }

    }
}
