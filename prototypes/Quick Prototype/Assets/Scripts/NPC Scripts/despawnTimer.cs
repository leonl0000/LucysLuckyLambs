using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class despawnTimer : MonoBehaviour
{
    public float timerStart;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = timerStart;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
