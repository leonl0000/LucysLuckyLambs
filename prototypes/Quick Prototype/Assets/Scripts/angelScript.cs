using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AngelState { DRIFTING, CHASING_SHEEP, ABDUCTING_SHEEP, ATTACKING_PLAYER };

public class angelScript : MonoBehaviour
{
    private Vector3 spawnPoint;
    private AngelState state;

    private float timeoutDriftChange;
    public float timeoutDriftChangeInitial = 3;

    public float driftRange = 30;
    public float driftOriginBias = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = transform.position;
        randomNextActivity();
    }

    void randomNextActivity()
    {
        // TODO other two branches
        state = AngelState.DRIFTING;
        startDrifting();
    }

    void startDrifting()
    {
        // Pick goal for random drift
        Vector3 driftDelta = new Vector3(Random.Range(-driftRange, driftRange), 0, Random.Range(-driftRange, driftRange));
        Vector3 driftGoal = transform.position + driftDelta;
        driftGoal += driftOriginBias * (spawnPoint - driftGoal);
        timeoutDriftChange = timeoutDriftChangeInitial;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == AngelState.DRIFTING)
        {
            timeoutDriftChange -= Time.deltaTime;
            if (timeoutDriftChange <= 0)
            {
                handleTimeoutDriftChange();
            }
            // TODO drift timeout
            // TODO check if wounded
        }
        // TODO other states
    }

    void handleTimeoutDriftChange()
    {
        // TODO
        timeoutDriftChange = timeoutDriftChangeInitial;
    }
}
