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
    private hellSceneManager hsm;
    private bool leftMove;
    private bool rightMove;
    private bool forwardMove;
    private bool backMove;
    private bool panRight;
    private bool panLeft;
    private bool[] abs = new bool[10];
    private KeyCode[] ALPHAS = {KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3,
        KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9};
    public float[] spellCoolDownTimeOut;
    public float[] spellCoolDownTimers = new float[10];
    private bool ab1;
    private bool ab2;
    private bool ab3;
    private bool ab4;
    private bool ab5;
    private bool jump;
    public bool wallInPlay;
    private float xangle;
    private float yangle;
    private bool panKey;
    public Constants.PanType pan_type;
    private Vector3 offsetAngle;
    public Transform cameraTransform;
    private FollowPlayer fp;
    

    void Start()
    {
        playerRB = this.GetComponent<Rigidbody>();
        pan_type = Constants.PanType.MOUSE;
        playerRB.AddForce(0, 200, 0);
        abilities = this.gameObject.GetComponent<Abilities>();
        num_jumps = max_jumps;
        hsm = GameObject.Find("GameManager").GetComponent<hellSceneManager>();
        fp = cam.GetComponent<FollowPlayer>();

        spellCoolDownTimeOut = new float[10] { 0, .5f, 0, .2f, 1f, 4f, 0, 0, 0, 0 };
        GameObject tempObj = new GameObject();
        tempObj.transform.position = transform.position;
        tempObj.transform.eulerAngles = transform.eulerAngles;
        cameraTransform = tempObj.transform;
        wallInPlay = false;
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
        for (int i = 0; i < 10; i++) abs[i] = Input.GetKey(ALPHAS[i]) && spellCoolDownTimers[i] <= 0;
        //ab1 = Input.GetKeyDown(KeyCode.Alpha1);
        //ab2 = Input.GetKey(KeyCode.Alpha2) || hsm.fireballDown;
        //ab3 = Input.GetKeyDown(KeyCode.Alpha3);
        //ab4 = Input.GetKeyDown(KeyCode.Alpha4);
        //ab5 = Input.GetKeyDown(KeyCode.Alpha5);

        if (pan_type == Constants.PanType.MOUSE) {
            //if(Input.mousePosition.x > 0 && Input.mousePosition.x < Screen.width)
            xangle += camSpeed * Input.GetAxis("Mouse X");
            //if(Input.mousePosition.y > 0 && Input.mousePosition.y < Screen.height)
            //yangle -= camSpeed * Input.GetAxis("Mouse Y") * 550/Screen.height;
            yangle = (Mathf.Max(Mathf.Min(Input.mousePosition.y, Screen.height), 0) / Screen.height - .5f) * -120;
        }
        xangle += camSpeed * ((panRight ? 1 : 0) + (panLeft ? -1 : 0));
        cameraTransform.eulerAngles = new Vector3(yangle, xangle, 0);
        transform.eulerAngles = new Vector3(0, xangle, 0);


        if (Input.GetKey("z")) fp.positionBack -= 10 * Time.deltaTime;
        if (Input.GetKey("x")) fp.positionBack += 10 * Time.deltaTime;

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
        
        playerRB.velocity += delta_velocity;
        //playerRB.AddForce(100* delta_velocity * Time.deltaTime, ForceMode.VelocityChange);

        // impose maximum on non-vertical velocity
        Vector3 horizontalVelocity = new Vector3(playerRB.velocity.x, 0, playerRB.velocity.z);
        if (horizontalVelocity.magnitude > max_velocity)
        {
            horizontalVelocity = horizontalVelocity.normalized * max_velocity;
            playerRB.velocity = new Vector3(horizontalVelocity.x, playerRB.velocity.y, horizontalVelocity.z);
        }


        if (jump && num_jumps > 0) {
            num_jumps--;
            playerRB.AddForce(0, 15, 0, ForceMode.VelocityChange);
        }

        if (abs[1]) {
            if(abilities.SpawnLure())
                spellCoolDownTimers[1] = spellCoolDownTimeOut[1];
        }        
        if (abs[2] || hsm.fireballDown)   {
            abilities.FireballKey();
        }  else if (abilities.isGrowingFireball)   {
            abilities.FireballRelease();
        }
        if (abs[3]) {
            if(abilities.spawnSheep())
                spellCoolDownTimers[3] = spellCoolDownTimeOut[3];
        }
        if(abs[4] && !wallInPlay) {
            if (abilities.trumpWall()) {
                wallInPlay = true;
                spellCoolDownTimers[4] = spellCoolDownTimeOut[4];
            }
        }
        if (abs[5]) {
            if(abilities.Lightning())
                spellCoolDownTimers[5] = spellCoolDownTimeOut[5];
        }
        for(int i = 0; i < 10; i++) {
            spellCoolDownTimers[i] = Mathf.Max(0, spellCoolDownTimers[i] - Time.deltaTime);
        }
    }
}
