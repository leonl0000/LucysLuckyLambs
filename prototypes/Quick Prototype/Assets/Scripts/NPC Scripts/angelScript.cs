using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AngelState { DRIFTING, CHASING_SHEEP, ABDUCTING_SHEEP, ATTACKING_PLAYER };

public class angelScript : MonoBehaviour
{
    public hellSceneManager hsm;
    private Vector3 spawnPoint;
    private AngelState state;
    private Rigidbody rb;
    public GameObject player;

    public float levitationHeight = 35;

    public float driftProbability = 0.4f;
    public float chaseProbability = 0.5f;
    // remaining probability is attackProbability

    private float timeoutDriftChange;
    public float timeoutDriftChangeInitial = 1;
    private float timeoutDriftStop;
    public float timeoutDriftStopInitial = 10;
    private Vector3 driftDir; // direction we're drifting in
    public float driftOriginBias = 0.2f;
    public float driftSpeed = 3;

    public float sheepChaseDist = 140;
    private GameObject sheepChaseTarget;
    private float timeoutChaseStop;
    public float timeoutChaseStopInitial = 30;
    private float timeoutChaseRedirect;
    public float timeoutChaseRedirectInitial = 1;
    public float chaseSpeed = 5;

    public float abductStartDist = 50; // must be greater then levitationHeight
    private float timeoutAbductStop;
    public float timeoutAbductStopInitial = 60;
    public float abductStopDist = 70;
    public float abductSpeed = 5;
    public float abductDoneDist = 10;

    public Color beamStartCol = Color.white;
    public Color beamEndCol = Color.cyan;
    public float beamStartAlpha = 0.7f;
    public float beamEndAlpha = 0.3f;
    private LineRenderer lineRenderer;
    public Material abductBeamMat;

    public float maxAutoAttackDist = 90;
    private float timeoutAttackStop;
    public float timeoutAttackStopInitial;
    public float attackMoveRandomStrength = 1;

    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = transform.position;
        rb = gameObject.GetComponent<Rigidbody>();
        hsm = FindObjectOfType<hellSceneManager>();

        // create abduction beam
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = abductBeamMat;
        lineRenderer.widthMultiplier = 0.2f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(beamStartCol, 0.0f), new GradientColorKey(beamEndCol, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(beamStartAlpha, 0.0f), new GradientAlphaKey(beamEndAlpha, 1.0f) }
        );
        lineRenderer.colorGradient = gradient;
        lineRenderer.positionCount = 0;

        randomNextActivity();
    }

    void randomNextActivity()
    {
        float randomNum = Random.value;

        if (randomNum < driftProbability)
        {
            state = AngelState.DRIFTING;
            startDrifting();
        }
        else if (randomNum < driftProbability + chaseProbability)
        {
            state = AngelState.CHASING_SHEEP;
            startChasing();
        }
        else
        {
            state = AngelState.ATTACKING_PLAYER;
            startAttacking();
        }
    }

    void startDrifting()
    {
        // Pick goal for random drift
        driftDir = Random.onUnitSphere;
        driftDir.y = 0; // no up/down drift
        if (driftDir.magnitude < 0.01)
            driftDir = new Vector3(1, 0, 1);
        driftDir /= driftDir.magnitude;

        timeoutDriftStop = timeoutDriftStopInitial;

        updateDriftDir();
    }

    void startChasing()
    {
        // Find random sheep within sheepChaseDist
        int numSheepConsidered = 0;
        if (hsm.sheepDict.Count > 0)
        {
            foreach (int index in hsm.sheepDict.Keys)
            {
                GameObject sheep = hsm.sheepDict[index];
                float distance = (sheep.transform.position - transform.position).magnitude;
                if (distance < sheepChaseDist)
                {
                    numSheepConsidered++;
                    if ((numSheepConsidered == 1) || (Random.value < (1 / numSheepConsidered)))
                        sheepChaseTarget = sheep;
                }
            }
        }

        // if no nearby sheep, just drift
        if (numSheepConsidered == 0)
        {
            state = AngelState.DRIFTING;
            startDrifting();
            return;
        }


        timeoutChaseStop = timeoutChaseStopInitial;

        updateChaseDir();
    }

    void startAttacking()
    {
        // Check if player close enough to spontaneously attack
        float dist = (player.transform.position - transform.position).magnitude;
        if (dist > maxAutoAttackDist)
        {
            randomNextActivity();
            return;
        }

        timeoutAttackStop = timeoutAttackStopInitial;

        updateAttack();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == AngelState.DRIFTING)
        {
            timeoutDriftStop -= Time.deltaTime;
            if (timeoutDriftStop <= 0)
            {
                randomNextActivity();
                return;
            }
            timeoutDriftChange -= Time.deltaTime;
            if (timeoutDriftChange <= 0)
                updateDriftDir();
            // TODO check if wounded
        }
        else if (state == AngelState.CHASING_SHEEP)
        {
            timeoutChaseStop -= Time.deltaTime;
            if (timeoutChaseStop <= 0)
            {
                randomNextActivity();
                return;
            }
            timeoutChaseRedirect -= Time.deltaTime;
            if (timeoutChaseRedirect <= 0)
                updateChaseDir();
            // TODO check if wounded
        }
        else if (state == AngelState.ABDUCTING_SHEEP)
        {
            timeoutAbductStop -= Time.deltaTime;
            if (timeoutAbductStop <= 0)
            {
                endAbducting();
                randomNextActivity();
                return;
            }
            updateAbduct();
        }
        else if (state == AngelState.ATTACKING_PLAYER)
        {
            timeoutAttackStop -= Time.deltaTime;
            if (timeoutAttackStop <= 0)
            {
                randomNextActivity();
                return;
            }
            updateAttack();
        }
    }

    void updateAttack()
    {
        // Find position we want to head to
        Vector3 goalPos = player.transform.position + new Vector3(0, levitationHeight, 0);
        // Find goal displacement, and normalize
        Vector3 goalDisplacement = goalPos - transform.position;
        goalDisplacement /= goalDisplacement.magnitude;
        // add random factor
        goalDisplacement += Random.onUnitSphere * attackMoveRandomStrength;
    }

    void updateDriftDir()
    {
        timeoutDriftChange = timeoutDriftChangeInitial;

        // position we-re headed towards
        Vector3 driftDest = transform.position + driftDir * driftSpeed * timeoutDriftChangeInitial;
        // change drift dest to be closer to spawn point, by interpolating
        driftDest += driftOriginBias * spawnPoint + (1 - driftOriginBias) * driftDest;
        // add random modification
        driftDest += Random.onUnitSphere;
        // update driftDir, and normalize
        driftDir = (driftDest - transform.position);
        driftDir.y = 0;
        driftDir /= driftDir.magnitude;
        // move in direction driftDir
        rb.velocity = driftDir * driftSpeed;
    }

    void updateChaseDir()
    {
        // check whether we can transition to abduction
        float dist = (sheepChaseTarget.transform.position - transform.position).magnitude;
        Debug.Log(string.Format("start abduction? dist is {0}", dist));
        if (dist <= abductStartDist)
        {
            state = AngelState.ABDUCTING_SHEEP;
            startAbducting();
            return;
        }

        timeoutChaseRedirect = timeoutChaseRedirectInitial;

        // compute goal
        Vector3 goal = sheepChaseTarget.transform.position;
        goal.y += levitationHeight;
        // move towards goal
        Vector3 chaseDir = goal - transform.position;
        chaseDir /= chaseDir.magnitude;
        rb.velocity = chaseDir * chaseSpeed;
    }

    void startAbducting()
    {
        Debug.Log("startAbducting");
        timeoutAbductStop = timeoutAbductStopInitial;

        lineRenderer.positionCount = 2;
        updateAbduct();
    }

    void updateAbduct()
    {
        // set velocity to 0
        rb.velocity = new Vector3(0, 0, 0);

        // Check whether sheep has somehow died
        if (sheepChaseTarget == null)
        {
            endAbducting();
            randomNextActivity();
            return;
        }

        // check whether sheep is out of abduction range
        Vector3 sheepToAngel = transform.position - sheepChaseTarget.transform.position;
        if (sheepToAngel.magnitude > abductStopDist)
        {
            endAbducting();
            randomNextActivity();
            return;
        }

        // check whether sheep is close enough to disappear
        if (sheepToAngel.magnitude < abductDoneDist)
        {
            // disappear sheep
            hsm.objectDrop(sheepChaseTarget);

            // TODO spawn particle effect

            // move out of abduction mode
            endAbducting();
            randomNextActivity();
            return;
        }

        // update beam
        var points = new Vector3[2];
        points[0] = transform.position;
        points[1] = sheepChaseTarget.transform.position;
        lineRenderer.SetPositions(points);

        // update sheep velocity
        Rigidbody sheepRB = sheepChaseTarget.GetComponent<Rigidbody>();
        sheepRB.velocity = (sheepToAngel / sheepToAngel.magnitude) * abductSpeed;

        // TODO turn towards sheep
    }

    void endAbducting()
    {
        // make linerenderer stop rendering
        lineRenderer.positionCount = 0;
    }
}
