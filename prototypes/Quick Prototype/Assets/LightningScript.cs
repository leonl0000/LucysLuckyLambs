using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningScript : MonoBehaviour
{
    public float dps = 5f;
    public float TimeOut = 5f;
    public float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = TimeOut;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0) {
            Destroy(transform.parent.gameObject);
        }
    }

    public void OnTriggerStay(Collider other) {
        HealthScript healthScript = other.GetComponent<HealthScript>();
        if (healthScript != null) healthScript.wound(dps * Time.fixedDeltaTime);
    }


}
