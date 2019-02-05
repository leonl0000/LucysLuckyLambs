using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnGateScript : MonoBehaviour
{
    public hellSceneManager hsm;

    //spawnTimeout sets interval between sheep, timer counts down from it
    public float spawnTimeout = 1f;
    private float timer;
    
    private Transform tf; //Position of the spawnGate


    void Start()
    {
        tf = this.gameObject.transform;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0) {
            if (hsm.spawnSheepAt(tf.position)) timer = spawnTimeout;
        }
    }
}
