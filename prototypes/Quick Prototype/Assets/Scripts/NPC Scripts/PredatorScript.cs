using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreditorScript : MonoBehaviour
{
    public hellSceneManager hsm;
    public float accelerationFactor = 30f;
    public float chasingTimeout = 10.0f;
    public float directionTimeout = 2.0f;
    public float velocityResetTimeout = .25f;
    public float stuckOnTerrainTimeout = 4f;
    public float maxSquaredDistance = 1000000f;
    public float speed = 10f;

    public Rigidbody rb;
    private float chaseTimer;
    private float directionTimer;
    private float velocityResetTimer;
    private Vector3 velocityTarget;
    private GameObject prey;
    public float terrainTimer;
    private Vector3 lastPosition;
    public float debugFloat;

    public GameObject bloodSplatter;

    void Start() {
        chaseTimer = chasingTimeout;
        directionTimer = directionTimeout;
        velocityResetTimer = velocityResetTimeout;
        terrainTimer = 0;
        rb = gameObject.GetComponent<Rigidbody>();
    }

    private GameObject getNewPrey() {
        GameObject nextPrey = null;
        float minSqDist = maxSquaredDistance;
        foreach (int index in hsm.sheepDict.Keys) {
            float dist = (gameObject.transform.position - hsm.sheepDict[index].transform.position).sqrMagnitude;
            Debug.Log(maxSquaredDistance);
            if (dist < minSqDist) {
                nextPrey = hsm.sheepDict[index];
                minSqDist = dist;
            }
        }
        chaseTimer = chasingTimeout;
        directionTimer = directionTimeout;
        return nextPrey;
    }

    // Update is called once per frame
    public void FixedUpdate() {
        if (chaseTimer < 0 || prey == null) prey = getNewPrey();
        else {
            chaseTimer -= Time.deltaTime;
            if (directionTimer < 0) {
                Vector3 direction = prey.transform.position - gameObject.transform.position;
                direction.y = direction.y > 0 ? direction.y : direction.y/2;
                direction = direction.normalized;
                velocityTarget = speed * direction;
                rb.velocity = velocityTarget;
                directionTimer = directionTimeout;
            } else {
                directionTimer -= Time.deltaTime;
                if (velocityResetTimer < 0) rb.velocity = velocityTarget;
                else velocityResetTimer -= Time.deltaTime;
            }
        }

        if ((rb.position - lastPosition).sqrMagnitude < 1 * Time.deltaTime) terrainTimer += Time.deltaTime;
        else terrainTimer = 0;
        if(terrainTimer > stuckOnTerrainTimeout && prey != null) {
            terrainTimer = 0;
            rb.AddForce(0, 250, 0, ForceMode.VelocityChange);
        }
        lastPosition = rb.position;
    }

    public void OnCollisionEnter(Collision collision) {        //Bounce on ground!
        if (collision.collider.tag == "ground") {
            rb.AddForce(0, 200*Time.deltaTime, 0, ForceMode.VelocityChange);
        } else   hsm.predatorCollision(gameObject, collision.gameObject);
    }

    public void wound(float damage, Transform site)
    {
        // TODO inflict damage, possibly die

        // Spawn blood splatter
        GameObject thisSplatter = Instantiate(bloodSplatter, site.position, site.rotation);
    }
}

