using UnityEngine;

public class sheepScript : MonoBehaviour {
    public Rigidbody rb;
    public hellSceneManager hsm;

    // x & z velocities will be set uniformly randomly in [-MAX_INITIAL_SPEED, MAX_INITIAL_SPEED]
    public float MAX_INITIAL_SPEED = 15;
    public int index;

    //Force applied whenever sheep hit the ground
    //public float sheepBounce = 15;

    public float maxSheepSpeed = 15;
    public float maxGoalSize = 15;

    public float playerApproachThreshold = 200 * 200; // squared distance
    public float playerAvoidThreshold = 10 * 10;
    public float boidNoise = 1f;

    private float moveCountdown;
    public float minStartCountdown = 0.5f;
    public float maxStartCountdown = 1.5f;
    // TODO eventually this will involve panic

    public GameObject bloodSplatter;

    public Vector3 goal;
    // Represents the direction the sheep wants to move in. Sheep takes steps in this direction.

    public float hopForce = 4; // force with which sheep hop up
    // TODO eventually this will involve panic

    private bool isOnGround = false;




    void Start() {
        hsm = FindObjectOfType<hellSceneManager>();
        rb.velocity = new Vector3(2 * MAX_INITIAL_SPEED * (Random.value - .5f), 0, 2 * MAX_INITIAL_SPEED * (Random.value - .5f));
        goal = new Vector3(0, 0, 0);
        moveCountdown = Random.Range(minStartCountdown, maxStartCountdown);
    }


    void FixedUpdate() {
        // Sheep rotate to face goal; this is before each frame to prevent jerky movements
        if (goal.sqrMagnitude > .01)
        {
            transform.rotation = Quaternion.LookRotation(goal, Vector3.up);
            // rotate by another -90 degrees y so it looks forward
            // TODO is there a way to do this in the unity object viewer? would be faster
            transform.Rotate(transform.rotation.x, transform.rotation.y - 90, transform.rotation.z);
        }
    }

    
    void Update() {
        //Check if fallen off
        if (rb.position.y < -10) hsm.objectDrop(this.gameObject);

        // Approach or avoid the player
        Vector3 playerDelta = hsm.player.transform.position - rb.position;
        if (playerDelta.sqrMagnitude < playerAvoidThreshold)
        {
            // Apply force away from player
            goal += -playerDelta * Time.deltaTime * hsm.playerBoidInfluence / Mathf.Max(playerDelta.sqrMagnitude, 1);
        }
        else if (playerDelta.sqrMagnitude < playerApproachThreshold)
        {
            // Apply force towards player
            //float force = (playerDelta * Time.deltaTime * hsm.playerBoidInfluence / playerDelta.sqrMagnitude).magnitude;
            //if (index%10==0) Debug.Log(string.Format("Sheep {0}: force {1:E3}, playerInf {2}", index, force, hsm.playerBoidInfluence));

            goal += playerDelta * Time.deltaTime * hsm.playerBoidInfluence / Mathf.Max(playerDelta.sqrMagnitude, 1);
        }

        goal.y = 0; // goal is on same plane as sheep

        // prevent goal from exceeding maximum magnitude
        if (goal.magnitude > maxGoalSize)
            goal = goal.normalized * maxGoalSize;

        // If countdown ended, move in direction of goal.
        if (moveCountdown <= 0 && isOnGround)
        {
            Vector3 goalHop = new Vector3(goal.x, hopForce, goal.z);
            rb.AddForce(goalHop, ForceMode.VelocityChange);
            isOnGround = false;
            if (rb.velocity.magnitude > maxSheepSpeed)
                rb.velocity = rb.velocity.normalized * maxSheepSpeed;

            moveCountdown = Random.Range(minStartCountdown, maxStartCountdown);
            
            // Add random goal change right after each hop
            Vector3 randomForce = Random.onUnitSphere;
            goal += randomForce * boidNoise;
            goal.y = 0; // goal is on same plane as sheep
        }
        // if a move countdown has passed but it hasn't hit the ground, just assume it's on ground
        // TODO fix this; leaving it out makes sheep freeze, letting them hop when not on ground causes flying sheep
        else if (moveCountdown <= 0 && !isOnGround)
        {
            moveCountdown = Random.Range(minStartCountdown, maxStartCountdown);
            isOnGround = true;
        }
        else
        {
            moveCountdown -= Time.deltaTime;
        }

        // Debug line to show goal; click on "gizmos" button to show/hide
        Debug.DrawLine(transform.position, transform.position + goal * 0.3f, Color.red);
    }

    private void OnCollisionEnter(Collision collision) {

        //Hitting ground
        if (collision.collider.tag == "ground") {
            // Bouncing -- disabled since sheep now hop
            // rb.AddForce(0, sheepBounce, 0, ForceMode.VelocityChange);

            isOnGround = true;
        }

    }

    public void wound(float damage, Transform site)
    {
        // TODO inflict damage, possibly die

        // Spawn blood splatter
        GameObject thisSplatter = Instantiate(bloodSplatter, site.position, site.rotation);
    }

}
