using UnityEngine;

public class sheepScript : MonoBehaviour {
    public Rigidbody rb;
    public hellSceneManager hsm;

    // x & z velocities will be set uniformly randomly in [-MAX_INITIAL_SPEED, MAX_INITIAL_SPEED]
    public float MAX_INITIAL_SPEED = 30;
    public int index;

    //Force applied whenever sheep hit the ground
    public float sheepBounce = 15;


    void Start() {
        hsm = FindObjectOfType<hellSceneManager>();
        rb.velocity = new Vector3(2 * MAX_INITIAL_SPEED * (Random.value - .5f), 0, 2 * MAX_INITIAL_SPEED * (Random.value - .5f));
    }

    
    void Update() {
        //Check if fallen off
        if (rb.position.y < -10) hsm.objectDrop(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision) {

        //Bounce on ground!
        if (collision.collider.tag == "ground") {
            rb.AddForce(0, sheepBounce, 0, ForceMode.VelocityChange);
        }

    }

}
