using UnityEngine;

public class sheepScript : MonoBehaviour {
    public Rigidbody rb;
    public hellSceneManager hsm;

    // x & z velocities will be set uniformly randomly in [-MAX_INITIAL_SPEED, MAX_INITIAL_SPEED]
    public float MAX_INITIAL_SPEED = 5;
    public int index;

    //Force applied whenever sheep hit the ground
    public float sheepBounce = 15;

    public float playerApproachThreshold = 200 * 200; // squared distance
    public float playerAvoidThreshold = 30 * 30;
    //public float boidNoise = 1;

    


    void Start() {
        hsm = FindObjectOfType<hellSceneManager>();
        rb.velocity = new Vector3(2 * MAX_INITIAL_SPEED * (Random.value - .5f), 0, 2 * MAX_INITIAL_SPEED * (Random.value - .5f));
    }

    
    void Update() {
        //Check if fallen off
        if (rb.position.y < -10) hsm.objectDrop(this.gameObject);

        // Approach or avoid the player
        Vector3 playerDelta = hsm.player.transform.position - rb.position;
        if (playerDelta.sqrMagnitude < playerAvoidThreshold)
        {
            // Apply force away from player
            rb.AddForce(-playerDelta * Time.deltaTime * hsm.playerBoidInfluence / playerDelta.sqrMagnitude, ForceMode.VelocityChange);
        }
        else if (playerDelta.sqrMagnitude < playerApproachThreshold)
        {
            // Apply force towards player
            //float force = (playerDelta * Time.deltaTime * hsm.playerBoidInfluence / playerDelta.sqrMagnitude).magnitude;
            //if (index%10==0) Debug.Log(string.Format("Sheep {0}: force {1:E3}, playerInf {2}", index, force, hsm.playerBoidInfluence));
            rb.AddForce(playerDelta * Time.deltaTime * hsm.playerBoidInfluence / playerDelta.sqrMagnitude, ForceMode.VelocityChange);
        }

        // Add random acceleration
        //Vector3 randomForce = Random.insideUnitCircle;
        //rb.AddForce(randomForce * boidNoise, ForceMode.VelocityChange);
    }

    private void OnCollisionEnter(Collision collision) {

        //Bounce on ground!
        if (collision.collider.tag == "ground") {
            rb.AddForce(0, sheepBounce, 0, ForceMode.VelocityChange);
        }

    }

}
