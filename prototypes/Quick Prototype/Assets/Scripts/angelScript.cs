using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AngelState { DRIFTING, CHASING_SHEEP, ABDUCTING_SHEEP, ATTACKING_PLAYER };

public class angelScript : MonoBehaviour
{
    private Vector3 spawnPoint;
    private AngelState state;
    private Rigidbody rb;

    private float timeoutDriftChange;
    public float timeoutDriftChangeInitial = 3;
    private Vector3 driftDir; // direction we're drifting in
    public float driftOriginBias = 0.2f;
    public float driftSpeed = 3;

    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = transform.position;
        rb = gameObject.GetComponent<Rigidbody>();
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
        driftDir = Random.onUnitSphere;
        driftDir.y = 0; // no up/down drift
        if (driftDir.magnitude < 0.01)
            driftDir = new Vector3(1, 0, 1);
        driftDir /= driftDir.magnitude;

        updateDriftDir();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == AngelState.DRIFTING)
        {
            timeoutDriftChange -= Time.deltaTime;
            if (timeoutDriftChange <= 0)
            {
                updateDriftDir();
            }
            // TODO drift timeout
            // TODO check if wounded
        }
        // TODO other states
    }

    void updateDriftDir()
    {
        timeoutDriftChange = timeoutDriftChangeInitial;

        // position we-re headed towards
        Vector3 driftDest = transform.position + driftDir * driftSpeed * timeoutDriftChangeInitial;
        // change drift dest to be closer to spawn point, by interpolating
        driftDest += driftOriginBias * spawnPoint + (1 - driftOriginBias) * driftDest;
        // add random modification
        driftDir += Random.onUnitSphere;
        driftDir.y = 0;
        // update driftDir, and normalize
        driftDir = (driftDest - transform.position);
        driftDir /= driftDir.magnitude;
        // move in direction driftDir
        rb.velocity = driftDir * driftSpeed;
    }
}
