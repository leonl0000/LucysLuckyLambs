using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredatorScript : MonoBehaviour
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
    public float health;

    public GameObject bloodSplatter;
    public HealthScript healthScript;

    void Start() {
        chaseTimer = chasingTimeout;
        directionTimer = directionTimeout;
        velocityResetTimer = velocityResetTimeout;
        terrainTimer = 0;
        health = 30;
        rb = gameObject.GetComponent<Rigidbody>();
        HealthScript.AddHealthScript(gameObject, 40, 4f, Resources.Load<GameObject>("BloodSplatter"));
    }

    public void onDeath() {
        Destroy(gameObject);
    }

    private GameObject getNewPrey() {
        GameObject nextPrey = null;
        float minSqDist = maxSquaredDistance;
        //Debug.Log("HSM " + hsm);
        //Debug.Log("SD " + hsm.sheepDict);
        foreach (int index in hsm.sheepDict.Keys) {
            float dist = (gameObject.transform.position - hsm.sheepDict[index].transform.position).sqrMagnitude;
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
                direction.y = direction.y > 0 ? direction.y : direction.y / 2;
                direction = direction.normalized;
                velocityTarget = speed * direction;
                rb.velocity = velocityTarget;
                directionTimer = directionTimeout;

                // rotate to face movement direction
                transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
                // rotate by another 90 degrees y so it looks forward
                // TODO is there a way to do this in the unity object viewer? would be faster
                transform.Rotate(transform.rotation.x, transform.rotation.y + 90, transform.rotation.z);
            } else {
                directionTimer -= Time.deltaTime;
                if (velocityResetTimer < 0) rb.velocity = velocityTarget;
                else velocityResetTimer -= Time.deltaTime;
            }
        }

        if ((rb.position - lastPosition).sqrMagnitude < 1 * Time.deltaTime) terrainTimer += Time.deltaTime;
        else terrainTimer = 0;
        if (terrainTimer > stuckOnTerrainTimeout && prey != null) {
            terrainTimer = 0;
            rb.AddForce(0, 250, 0, ForceMode.VelocityChange);
        }
        lastPosition = rb.position;

    }

    public void OnCollisionEnter(Collision collision) {        //Bounce on ground!
        switch (collision.collider.tag) {
            case "ground":
                rb.AddForce(0, 200 * Time.deltaTime, 0, ForceMode.VelocityChange);
                break;

            case "fireball":
                break;

            default:
                hsm.predatorCollision(gameObject, collision.gameObject);
                break;
        }
    }

    //public void wound(float damage, Transform site) {
    //    GameObject thisSplatter = Instantiate(, site.position, site.rotation);
    //    health -= damage;
    //    if (health < 0) Destroy(gameObject);
    //}
}

