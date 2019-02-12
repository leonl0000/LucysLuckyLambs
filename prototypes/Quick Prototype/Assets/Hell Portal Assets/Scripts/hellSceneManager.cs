using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hellSceneManager : MonoBehaviour {

    public GameObject player;
    public float mana;
    public float health;
    public int numSheepDropped;
    public GameObject sheep;



    public Dictionary<int, GameObject> sheepDict;
    private int nextIndex;

    public Dictionary<int, GameObject> lureDict;



    public float playerBoidInfluence = 150f; //multiplied onto the force each sheep gets applied

    public float boidSeparateThreshold;
    public float boidCohereThreshold;
    public float boidAlignThreshold;
    // note we must have cohere > align, and cohere > separate
    private float boidCohereThresholdSQ;

    public float boidSeparationStrength;
    public float boidCoherenceStrength;
    public float boidAlignmentStrength;

    public float lureRange;
    public float lureStrength;



    public void Start() {
        mana = 50f;
        health = 100f;
        numSheepDropped = 0;
        sheepDict = new Dictionary<int, GameObject>();
        lureDict = new Dictionary<int, GameObject>();
        nextIndex = 0;
    }


    //HELLGATE Objects
    public void objectEnterHellgate(GameObject o, hellgateScript hellGate) {
        switch(o.tag) {
            case "sheep":
                Destroy(o);
                sheepDict.Remove(o.GetComponent<sheepScript>().index);
                //Debug.Log(string.Format("Sheep {0} SACRIFICED", o.GetComponent<sheepScript>().index));
                mana += 100;
                hellGate.numSacrificed += 1;
                break;

            case "Player":
                health -= 5f * Time.deltaTime;
                if (health < 0) Destroy(o);
                break;

        }
    }

    //Deal with objects that fall off the edge
    public void objectDrop(GameObject o) {
        if (o.tag == "sheep") {
            Destroy(o);
            sheepDict.Remove(o.GetComponent<sheepScript>().index);
            //Debug.Log(string.Format("Sheep {0} dropped", o.GetComponent<sheepScript>().index));
            numSheepDropped += 1;
        }
    }

    public bool spawnSheepAt(Vector3 pos) {
        if (mana > 0) {
            mana--;
            GameObject s = Instantiate(sheep, pos, sheep.transform.rotation);
            s.GetComponent<sheepScript>().index = nextIndex;
            sheepDict[nextIndex] = s;
            nextIndex++;
            //Debug.Log(string.Format("Sheep {0} Born", s.GetComponent<sheepScript>().index));
            return true;
        }
        return false;
    }

    void Update() {
        updateBoids();
    }

    // Update all boids' velocities
    public void updateBoids() {
        // Algorithm overview:
        // Iterate over every pair of boids
        // For every pair, compute distance, and then apply the three rules (separation, cohesion, alignment)
        boidCohereThresholdSQ = boidCohereThreshold * boidCohereThreshold;

        // handle boid pair interactions
        foreach (int i in sheepDict.Keys) { 
            foreach (int j in sheepDict.Keys) { 
                if (i<j)
                    // Debug.Log(string.Format("Interacting: sheep {0} -- {1}", i, j));
                    updateBoidPair(sheepDict[i], sheepDict[j]);
            }
        }

        // handle single boid effects
        foreach (int i in sheepDict.Keys) {
            // lure interactions
            foreach (int j in lureDict.Keys)
                lureAttract(sheepDict[i], lureDict[j]);
        }
    }

    public void lureAttract (GameObject sheep, GameObject lure) {
        Vector3 posSheep = sheep.transform.position;
        Vector3 posLure = lure.transform.position;
        Vector3 sheepToLure = posLure - posSheep;
        var distance = sheepToLure.magnitude;
        if (distance < lureRange) {
            sheep.GetComponent<Rigidbody>().AddForce(sheepToLure * lureStrength * Time.deltaTime);
        }
    }

    // Update the velocities of 2 boids to account for their effect on each other
    public void updateBoidPair(GameObject a, GameObject b)
    {
        Rigidbody bodyA = a.GetComponent<Rigidbody>();
        Rigidbody bodyB = b.GetComponent<Rigidbody>();
        Vector3 posA = a.transform.position;
        Vector3 posB = b.transform.position;
        Vector3 aToB = posB - posA;
        var distanceSQ = aToB.sqrMagnitude;

        // if too far to cohere, then no interaction
        // Minor optimization: don't do square root if not necessary
        if (distanceSQ > boidCohereThresholdSQ)
            return;

        var distance = Mathf.Sqrt(distanceSQ);

        Vector3 aToBNormalized = aToB / distance;
        Vector3 planeNormal = new Vector3(0, 1, 0);
        Vector3 aToBNormalizedXZ = Vector3.ProjectOnPlane(aToBNormalized, planeNormal);
            // a to b vector, with y dimension set to zero so boids don't try to move up/

        // either separate or cohere, depending on boidSeparateThreshold
        if (distance < boidSeparateThreshold)
        {
            // apply separation rule
            bodyA.AddForce(-aToBNormalizedXZ * boidSeparationStrength * Time.deltaTime);
            bodyB.AddForce( aToBNormalizedXZ * boidSeparationStrength * Time.deltaTime);
            // also TODO separation rule for player, so boids don't crowd it
        }
        else
        {
            // apply coherence rule
            bodyA.AddForce( aToBNormalizedXZ * boidCoherenceStrength * Time.deltaTime); // TODO scale down with distance
            bodyB.AddForce(-aToBNormalizedXZ * boidCoherenceStrength * Time.deltaTime);
        }

        if (distance < boidAlignThreshold)
        {
            // apply alignment rule
            Vector3 temp = Vector3.Slerp(bodyA.velocity, bodyB.velocity, boidAlignmentStrength * Time.deltaTime); // TODO scale down with distance
            bodyB.velocity = Vector3.Slerp(bodyB.velocity, bodyA.velocity, boidAlignmentStrength * Time.deltaTime);
            bodyA.velocity = temp;
        }
    }

}
