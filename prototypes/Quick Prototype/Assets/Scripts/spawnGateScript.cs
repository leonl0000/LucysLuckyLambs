using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnGateScript : MonoBehaviour
{
    public hellSceneManager hsm;

    //spawnTimeout sets interval between sheep, timer counts down from it
    public float spawnTimeout = 1f;
    private float timer = 1f;

    private Transform tf; //Position of the spawnGate


    void Start()
    {
        tf = this.gameObject.transform;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0) {
            bool b =hsm.spawnSheepAt(tf.position);
            Debug.Log(b);
            if (b) timer = spawnTimeout;
        }
    }
}
