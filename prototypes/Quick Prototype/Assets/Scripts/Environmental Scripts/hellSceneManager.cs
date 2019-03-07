using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class hellSceneManager : MonoBehaviour {

    public GameObject player;
    public Abilities playerAbilities;
    private PlayerMovement playerMovement;
    public bool fireballDown;
    public float mana;
    public float health;
    public int numSheepDropped;
    public int numSheepEaten;
    public GameObject sheep;
    public Transform spawnGate;


    public Dictionary<int, GameObject> sheepDict;
    private int nextSheepIndex;

    public Dictionary<int, GameObject> lureDict;

    public Dictionary<int, GameObject> angelDict;
    public int nextAngelIndex;

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

    public float playerApproachThreshold = 200;
    public float playerAvoidThreshold = 10;
    public float boidNoise = 1f;

    public float playerBoidFactor = 0.3f;
    public float coherenceAvoidanceBoidFactor = 0.2f;
    public float alignmentBoidFactor = 0.2f;
    public float lureBoidFactor = 0.3f;



    public void Start() {
        mana = 50f;
        health = 100f;
        numSheepDropped = 0;
        numSheepEaten = 0;
        sheepDict = new Dictionary<int, GameObject>();
        lureDict = new Dictionary<int, GameObject>();
        angelDict = new Dictionary<int, GameObject>();
        nextSheepIndex = 0;
        playerAbilities = player.GetComponent<Abilities>();
        playerMovement = player.GetComponent<PlayerMovement>();
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
                spawnSheep();
                break;

            case 4:
                if (!playerMovement.wallInPlay) playerAbilities.trumpWall();
                break;
        }
    }

    void FixedUpdate() {
        if (mana > 1000) {
            nextLevel();
        }
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

    public bool spawnSheep() {
        return spawnSheepAt(spawnGate.position);

    }

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
    /* Called with a sheep's index. Returns its goal based on current game state.
     This is combined with sheep's current goal to produce new goal. */
    public Vector3 getSheepGoal(int i)
    {

        GameObject sheepI = sheepDict[i];
        Vector3 posI = sheepI.transform.position;

        Vector3 coherenceAvoidanceGoal = new Vector3(0, 0, 0);
        float numCohereAvoid = 0;
        Vector3 alignmentGoal = new Vector3(0, 0, 0);
        float numAlignment = 0;
        Vector3 playerGoal = new Vector3(0, 0, 0);
        Vector3 lureGoal = new Vector3(0, 0, 0);
        float numLures = 0;

        // First, compute all sheep-pair effects together
        foreach (int j in sheepDict.Keys)
        {
            if (i == j)
                continue;

            GameObject sheepJ = sheepDict[j];
            Vector3 posJ = sheepJ.transform.position;
            Vector3 ItoJ = posJ - posI;
            float distance = ItoJ.magnitude;

            Vector3 ItoJnormalized = ItoJ / distance;
            Vector3 ItoJnormflat = new Vector3(ItoJnormalized.x, 0, ItoJnormalized.z);

            // Either separate or cohere, depending on distance
            if (distance < boidSeparateThreshold)
            {
                // apply separation rule
                coherenceAvoidanceGoal += -ItoJnormflat * boidSeparationStrength / Mathf.Min(1, distance);
                numCohereAvoid += 1 / Mathf.Min(1, distance);
            }
            else if (distance < boidCohereThreshold)
            {
                // apply coherence rule
                coherenceAvoidanceGoal += ItoJnormflat * boidCoherenceStrength * (1 - (distance / boidCohereThreshold));
                numCohereAvoid += (1 - (distance / boidCohereThreshold));
            }

            // Compute alignment effect
            if (distance < boidAlignThreshold)
            {
                Vector3 sheepJgoal = sheepJ.GetComponent<sheepScript>().goal;
                alignmentGoal += (1 - (distance / boidAlignThreshold)) * sheepJgoal / Mathf.Max(1, sheepJgoal.magnitude);
                numAlignment += (1 - (distance / boidAlignThreshold));
            }

        }

        // Compute player-attraction effect
        Vector3 toPlayer = player.transform.position - sheepI.transform.position;
        if (toPlayer.magnitude < playerAvoidThreshold)
            // Apply force away from player
            playerGoal = -toPlayer * playerBoidInfluence / Mathf.Max(toPlayer.magnitude, 1);
        else if (toPlayer.magnitude < playerApproachThreshold)
            // Apply force towards player
            playerGoal = toPlayer * playerBoidInfluence * (1 - (toPlayer.magnitude / playerApproachThreshold));

        // Compute lure-attraction effect
        foreach (int j in lureDict.Keys)
        {
            Vector3 toLure = lureDict[j].transform.position - posI;
            var distance = toLure.magnitude;
            if (distance < lureRange)
            {
                lureGoal += toLure * lureStrength * (1 - (distance / lureRange)); // note no decay of lure strength over time, until it disappears
                numLures += (1 - (distance / lureRange));
            }
        }

        // Combine all the effects together to generate new goal
        Vector3 newGoal = new Vector3(0, 0, 0);
        // normalize each factor
        if (numCohereAvoid != 0)
            coherenceAvoidanceGoal /= numCohereAvoid;
        if (numAlignment != 0)
            alignmentGoal /= numAlignment;
        // lureGoal /= numLures; // dividing makes 2 lures as effective as one
        // add in each factor, with weight
        newGoal += playerBoidFactor * playerGoal;
        newGoal += coherenceAvoidanceBoidFactor * coherenceAvoidanceGoal;
        newGoal += alignmentBoidFactor * alignmentGoal;
        newGoal += lureBoidFactor * lureGoal;
        // Add random goal change
        Vector3 randomForce = Random.onUnitSphere;
        newGoal += randomForce * boidNoise;
        newGoal.y = 0; // goal is on same plane as sheep
        Debug.AssertFormat(!float.IsNaN(newGoal.x), "{0} {1} {2} {3}", playerGoal, coherenceAvoidanceGoal, alignmentGoal, lureGoal);
        Debug.AssertFormat(!float.IsNaN(newGoal.z), "{0} {1} {2} {3}", playerGoal, coherenceAvoidanceGoal, alignmentGoal, lureGoal);
        return newGoal;
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
