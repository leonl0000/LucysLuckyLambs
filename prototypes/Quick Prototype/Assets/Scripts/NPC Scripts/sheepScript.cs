﻿using UnityEngine;

public class sheepScript : MonoBehaviour {
    public Rigidbody rb;
    public hellSceneManager hsm;
    private HealthScript healthScript;

    // x & z velocities will be set uniformly randomly in [-MAX_INITIAL_SPEED, MAX_INITIAL_SPEED]
    public float MAX_INITIAL_SPEED = 15;
    public int index;

    //Force applied whenever sheep hit the ground
    //public float sheepBounce = 15;

    public float maxSheepSpeed = 15;
    public float maxGoalSize = 15;

    private float moveCountdown;
    public float minStartCountdown = 0.7f;
    public float maxStartCountdown = 2.0f;
    // TODO eventually this will involve panic

    public GameObject bloodSplatter;

    public Vector3 goal;
    // Represents the direction the sheep wants to move in. Sheep takes steps in this direction.

    public float hopForce = 4; // force with which sheep hop up
    // TODO eventually this will involve panic

    public bool isOnGround = false;

    public float oldGoalPersistence = 0.5f;
    public float newGoalStrength = 0.5f;

    private float max_health = 5;
    
    public float fallingThreshold = 0.05f;

    public float woundCollisionMomentum = 25 * 10;
    public float lethalCollisionMomentum = 40 * 10;
    
    public float woundDropSpeed = 10;
    public float lethalDropSpeed = 40;


    void Start() {
        hsm = FindObjectOfType<hellSceneManager>();
        rb.velocity = new Vector3(2 * MAX_INITIAL_SPEED * (Random.value - .5f), 0, 2 * MAX_INITIAL_SPEED * (Random.value - .5f));
        goal = new Vector3(0, 0, 0);
        moveCountdown = Random.Range(minStartCountdown, maxStartCountdown);
        healthScript = HealthScript.AddHealthScript(gameObject, max_health, 4f, Resources.Load<GameObject>("BloodSplatter"), null, DeathFunction);
        hsm.registerSheep(gameObject);
    }

    public bool DeathFunction() {
        hsm.sheepDict.Remove(index);
        return true;
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

    public Vector3 gg;
    public Vector3 g2;
    
    void Update() {
        //Check if fallen off
        if (rb.position.y < -10) hsm.objectDrop(this.gameObject);

        // If countdown ended, move in direction of goal.
        if (moveCountdown <= 0 && isOnGround)
        {
            Vector3 goalHop = new Vector3(goal.x, hopForce, goal.z);
            g2 = goalHop;
            rb.AddForce(goalHop, ForceMode.VelocityChange);
            isOnGround = false;

            Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            if (horizontalVelocity.magnitude > maxSheepSpeed) {
                horizontalVelocity = horizontalVelocity.normalized * maxSheepSpeed;
                rb.velocity = new Vector3(horizontalVelocity.x, rb.velocity.y, horizontalVelocity.z);
            }

            moveCountdown = Random.Range(minStartCountdown, maxStartCountdown);

            goal.y = 0; // goal is on same plane as sheep
        }
        // if a move countdown has passed but it hasn't hit the ground, just assume it's on ground
        // TODO fix this; leaving it out makes sheep freeze, letting them hop when not on ground causes flying sheep
        else if (moveCountdown <= 0 && !isOnGround && rb.velocity.y > fallingThreshold)
        {
            moveCountdown = Random.Range(minStartCountdown, maxStartCountdown);
            isOnGround = true;
            afterHop();
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
            // do damage when sheep hit the ground too fast
            /* float collisionSpeed = collision.relativeVelocity.magnitude;
            if (collisionSpeed > woundDropSpeed)
            {
                float damage = collisionSpeed / lethalDropSpeed;
                Debug.Log(string.Format("collision speed {0} damage {1}", collisionSpeed, damage));
                healthScript.wound(damage, collision.transform);
            } */

            isOnGround = true;

            afterHop();
        } else {
            // do damage if momentum is too great
            /* Rigidbody rbHit = collision.collider.GetComponent<Rigidbody>();
            if (rbHit != null) {
                float collisionMass = rbHit.mass;
                float collisionSpeed = collision.relativeVelocity.magnitude;
                float collisionMomentum = collisionMass * collisionSpeed;
                if (collisionMomentum > woundCollisionMomentum) {
                    float damage = collisionMomentum / lethalCollisionMomentum;
                    Debug.Log(string.Format("collision momentum {0} damage {1}", collisionMomentum, damage));
                    healthScript.wound(damage, collision.transform);
                }
            } */
        }
    }

    public Vector3 g3;

    /* Called after every hop. Sets new goal. */
    private void afterHop()
    {
        // After every hop, update goal as interpolation of old and new goals
        Vector3 newGoal = hsm.getSheepGoal(index);
        gg = newGoal;
        goal = goal * oldGoalPersistence + newGoal * newGoalStrength;
        g3 = goal;

        goal.y = 0; // goal is on same plane as sheep

        // prevent goal from exceeding maximum magnitude
        if (goal.magnitude > maxGoalSize)
            goal = goal.normalized * maxGoalSize;
    }

}
