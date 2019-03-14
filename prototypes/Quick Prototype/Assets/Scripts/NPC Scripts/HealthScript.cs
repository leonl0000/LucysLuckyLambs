using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
    public float Health;
    public float StartHealth;
    GameObject woundObject;
    Action<float> WoundAction;
    Func<bool> DeathFunction;
    public GameObject HealthBarObject;
    public Image HealthBar;
    

    public static HealthScript AddHealthScript(GameObject parent, float startHealth, float healthBarHeight, GameObject woundObject = null, 
        Action<float> WoundAction = null, Func<bool> DeathFunction = null) {
        HealthScript healthScript = parent.AddComponent<HealthScript>();

        healthScript.HealthBarObject = Instantiate(Resources.Load<GameObject>("HealthBarObject"));
        healthScript.HealthBarObject.transform.Find("HealthCanvas").GetComponent<HealthBarPosScript>().height = healthBarHeight;
        healthScript.HealthBarObject.transform.SetParent(parent.transform, false);
        healthScript.HealthBarObject.transform.localScale = Vector3.Scale(healthScript.HealthBarObject.transform.localScale,
            new Vector3(1 / parent.transform.localScale.x, 1 / parent.transform.localScale.y, 1 / parent.transform.localScale.z));
        healthScript.HealthBar = healthScript.HealthBarObject.transform.Find("HealthCanvas").Find("HealthBar").GetComponent<Image>();
        healthScript.HealthBarObject.SetActive(false);


        healthScript.StartHealth = startHealth;
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
        HealthBarObject.SetActive(true);
        if (woundObject != null && site != null) Instantiate(woundObject, site.position, site.rotation);
        WoundAction?.Invoke(damage);
        if (Health <= 0) {
            bool destroySelf = true;
            if (DeathFunction != null) destroySelf = DeathFunction();
            if (destroySelf) Destroy(gameObject);
        }
        if (HealthBar != null) HealthBar.fillAmount = Health / StartHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
