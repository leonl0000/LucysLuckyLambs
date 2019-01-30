using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnGateScript : MonoBehaviour
{
    public hellSceneManager hsm;
    public GameObject sheep;

    //spawnTimeout sets interval between sheep, timer counts down from it
    public float spawnTimeout = 1f;
    private float timer;
    
    private Transform tf; //Position of the spawnGate

    //Spawns sheep and subtracts mana
    void spawnSheep() {
        if(hsm.mana > 0) {
        Instantiate(sheep, tf.position, sheep.transform.rotation);
        hsm.mana--;
        timer = spawnTimeout;
        }
    }


    void Start()
    {
        tf = this.gameObject.transform;
        Instantiate(sheep, tf.position, sheep.transform.rotation);
        hsm.mana--;
        timer = spawnTimeout;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0) spawnSheep();
    }
}
