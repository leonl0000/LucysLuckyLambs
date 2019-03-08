using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    public float Health;
    GameObject woundObject;
    Action<float> WoundAction;
    Func<bool> DeathFunction;

    public static HealthScript AddHealthScript(GameObject parent, float startHealth, GameObject woundObject = null, 
        Action<float> WoundAction = null, Func<bool> DeathFunction = null) {
        HealthScript healthScript = parent.AddComponent<HealthScript>();
        healthScript.Health = startHealth;
        healthScript.woundObject = woundObject;
        healthScript.WoundAction = WoundAction;
        healthScript.DeathFunction = DeathFunction;
        return healthScript;
    }


      
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void wound (float damage, Transform site = null) {
        Health -= damage;
        if (woundObject != null && site != null) Instantiate(woundObject, site.position, site.rotation);
        WoundAction?.Invoke(damage);
        if (Health < 0) {
            bool destroySelf = true;
            if (DeathFunction != null) destroySelf = DeathFunction();
            if (destroySelf) Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
