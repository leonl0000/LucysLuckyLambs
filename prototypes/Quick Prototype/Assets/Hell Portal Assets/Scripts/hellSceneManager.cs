using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hellSceneManager : MonoBehaviour {
    public float mana;
    public int numSheepDropped;
    public GameObject sheep;

    public Dictionary<int, GameObject> sheepDict;
    private int nextIndex;

    public GameObject player;
    public float playerBoidInfluence = 150f; //multiplied onto the force each sheep gets applied

    public float boidSeparateThreshold;
    public float boidCohereThreshold;
    public float boidAlignThreshold;
    // note we must have cohere > align, and cohere > separate

    public float boidSeparationStrength;
    public float boidCoherenceStrength;
    public float boidAlignmentStrength;



    public void Start() {
        mana = 50;
        numSheepDropped = 0;
        sheepDict = new Dictionary<int, GameObject>();
        nextIndex = 0;
    }


    //HELLGATE Objects
    public void objectEnterHellgate(GameObject o, hellgateScript hellGate) {
        if (o.tag == "sheep") {
            Destroy(o);
            sheepDict.Remove(o.GetComponent<sheepScript>().index);
            //Debug.Log(string.Format("Sheep {0} SACRIFICED", o.GetComponent<sheepScript>().index));
            mana += 100;
            hellGate.numSacrificed += 1;
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

        for (int i = 0; i < sheepDict.Count - 1; i++)
        {
            for (int j = i + 1; j < sheepDict.Count; j++)
            {
                // Debug.Log(string.Format("Interacting: sheep {0} -- {1}", i, j));
                updateBoidPair(sheepDict[i], sheepDict[j]);
            }
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
        var distance = aToB.magnitude;

        // if too far to cohere, then no interaction
        if (distance > boidCohereThreshold)
            return;

        Vector3 aToBNormalized = aToB / distance;
        Vector3 planeNormal = new Vector3(0, 1, 0);
        Vector3 aToBNormalizedXZ = Vector3.ProjectOnPlane(aToBNormalized, planeNormal);
            // a to b vector, with y dimension set to zero so boids don't try to move up/down

        // either separate or cohere, depending on boidSeparateThreshold
        if (distance < boidSeparateThreshold)
        {
            // apply separation rule
            bodyA.AddForce(-aToBNormalizedXZ * boidSeparationStrength);
            bodyB.AddForce( aToBNormalizedXZ * boidSeparationStrength);
            // also TODO separation rule for player, so boids don't crowd it
        }
        else
        {
            // apply coherence rule
            bodyA.AddForce( aToBNormalizedXZ * boidCoherenceStrength); // TODO scale down with distance
            bodyB.AddForce(-aToBNormalizedXZ * boidCoherenceStrength);
        }

        if (distance < boidAlignThreshold)
        {
            // apply alignment rule
            // TODO
        }
    }

}
