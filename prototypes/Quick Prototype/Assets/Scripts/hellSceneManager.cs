using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class hellSceneManager : MonoBehaviour {

    public GameObject player;
    public Abilities playerAbilities;
    public float mana;
    public float health;
    public int numSheepDropped;
    public int numSheepEaten;
    public GameObject sheep;
    public Transform spawnGate;
    private bool fireballDown;


    public Dictionary<int, GameObject> sheepDict;
    private int nextSheepIndex;

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
        numSheepEaten = 0;
        sheepDict = new Dictionary<int, GameObject>();
        lureDict = new Dictionary<int, GameObject>();
        nextSheepIndex = 0;
        playerAbilities = player.GetComponent<Abilities>();
        if (SaveSystem.saveSlot != 0) load(SaveSystem.saveSlot);
        fireballDown = false;
    }

    public void triggerAbility(int abNum) {
        switch(abNum) {
            case 1:
                playerAbilities.SpawnLure();
                break;

            case 2:
                fireballDown = true;
                break;

            case -2:
                fireballDown = false;
                break;

            case 3:
                spawnSheepAt(spawnGate.position);
                break;
        }
    }

    void FixedUpdate() {
        updateBoids();
        if (mana > 1000) {
            nextLevel();
        }
        if(fireballDown) {
            playerAbilities.FireballKey();
        } else if (playerAbilities.isGrowingFireball)
            playerAbilities.FireballRelease();
    }

    #region Collisions and Falls

    //HELLGATE Objects
    public void objectEnterHellgate(GameObject o, hellgateScript hellGate) {
        switch(o.tag) {
            case "sheep":
                sheepDict.Remove(o.GetComponent<sheepScript>().index);
                Destroy(o);
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
            sheepDict.Remove(o.GetComponent<sheepScript>().index);
            Destroy(o);
            //Debug.Log(string.Format("Sheep {0} dropped", o.GetComponent<sheepScript>().index));
            numSheepDropped += 1;
        }
    }

    public void predatorCollision(GameObject pred, GameObject prey) {
        if(prey.tag == "sheep") {
            numSheepEaten++;
            sheepDict.Remove(prey.GetComponent<sheepScript>().index);
            Destroy(prey);
        }
    }
    #endregion

    public bool spawnSheepAt(Vector3 pos) {
        if (mana > 0) {
            mana--;
            GameObject s = Instantiate(sheep, pos, sheep.transform.rotation);
            s.GetComponent<sheepScript>().index = nextSheepIndex;
            sheepDict[nextSheepIndex] = s;
            nextSheepIndex++;
            //Debug.Log(string.Format("Sheep {0} Born", s.GetComponent<sheepScript>().index));
            return true;
        }
        return false;
    }


    #region Boids and Lures
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
            var sheep = sheepDict[i];
            // lure interactions
            foreach (int j in lureDict.Keys)
                lureAttract(sheep, lureDict[j]);
            // max speed
            //if (sheep.GetComponent<Rigidbody>().velocity.magnitude > maxSheepSpeed)
            //    sheep.GetComponent<Rigidbody>().velocity = sheep.GetComponent<Rigidbody>().velocity.normalized * maxSheepSpeed;
        }
    }

    public void lureAttract (GameObject sheep, GameObject lure) {
        Vector3 posSheep = sheep.transform.position;
        Vector3 posLure = lure.transform.position;
        Vector3 sheepToLure = posLure - posSheep;
        var distance = sheepToLure.magnitude;
        if (distance < lureRange) {
            sheep.GetComponent<sheepScript>().goal += sheepToLure * lureStrength * Time.deltaTime;
        }
    }

    // Update the velocities of 2 boids to account for their effect on each other
    public void updateBoidPair(GameObject a, GameObject b)
    {
        Rigidbody bodyA = a.GetComponent<Rigidbody>();
        Rigidbody bodyB = b.GetComponent<Rigidbody>();
        sheepScript scriptA = a.GetComponent<sheepScript>();
        sheepScript scriptB = b.GetComponent<sheepScript>();
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
            scriptA.goal += -aToBNormalizedXZ * boidSeparationStrength * Time.deltaTime / Mathf.Min(1, distance);
            scriptB.goal += aToBNormalizedXZ * boidSeparationStrength * Time.deltaTime / Mathf.Min(1, distance);
            // also TODO separation rule for player, so boids don't crowd it
        }
        else
        {
            // apply coherence rule
            scriptA.goal += aToBNormalizedXZ * boidCoherenceStrength * Time.deltaTime; // TODO scale down with distance
            scriptB.goal += -aToBNormalizedXZ * boidCoherenceStrength * Time.deltaTime;
        }

        if (distance < boidAlignThreshold)
        {
            // apply alignment rule
            Vector3 temp = scriptA.goal + Vector3.Slerp(scriptA.goal, scriptB.goal, boidAlignmentStrength * Time.deltaTime); // TODO scale down with distance
            scriptB.goal += Vector3.Slerp(scriptB.goal, scriptA.goal, boidAlignmentStrength * Time.deltaTime);
            scriptA.goal = temp;
        }
    }

    #endregion


    #region Save, Load, and Scene Navigation
    public void nextLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void save(int slot) {
        SaveSystem.SaveGame(this, slot);
    }

    public void load(int slot) {
        SaveData data = SaveSystem.LoadGame(slot);
        mana = data.mana;
        health = data.health;
        numSheepDropped = data.numSheepDropped;
        numSheepEaten = data.numSheepEaten;
        player.transform.position = new Vector3(data.playerPos[0], data.playerPos[1], data.playerPos[2]);

    }
    #endregion

}
