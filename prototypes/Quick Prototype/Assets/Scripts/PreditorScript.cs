using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreditorScript : MonoBehaviour
{
    public hellSceneManager hsm;
    public float accelerationFactor = 30f;
    public float chasingTimeout = 10f;
    public float maxSquaredDistance = 1000000f;

    public Rigidbody rb;
    private float timer;
    private GameObject prey;

    void Start() {
        timer = chasingTimeout;
        rb = gameObject.GetComponent<Rigidbody>();
    }

    private GameObject getNewPrey() {
        GameObject nextPrey = null;
        float minSqDist = maxSquaredDistance;
        foreach (int index in hsm.sheepDict.Keys) {
            float dist = (gameObject.transform.position - hsm.sheepDict[index].transform.position).sqrMagnitude;
            if (dist < minSqDist) {
                nextPrey = hsm.sheepDict[index];
                minSqDist = dist;
            }
        }
        timer = chasingTimeout;
        return nextPrey;
    }

    // Update is called once per frame
    public void FixedUpdate() {
        if (timer < 0 || prey == null) prey = getNewPrey();
        else {
            timer -= Time.deltaTime;
            Vector3 direction = (prey.transform.position - gameObject.transform.position).normalized;
            rb.AddForce(direction * Time.deltaTime * accelerationFactor, ForceMode.VelocityChange);
        }
    }

    public void OnCollisionEnter(Collision collision) {        //Bounce on ground!
        if (collision.collider.tag == "ground") {
            rb.AddForce(0, 200*Time.deltaTime, 0, ForceMode.VelocityChange);
        } else   hsm.predatorCollision(gameObject, collision.gameObject);
    }
}

