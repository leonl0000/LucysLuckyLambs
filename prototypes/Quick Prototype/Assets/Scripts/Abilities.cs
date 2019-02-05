using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilities : MonoBehaviour {

    public GameObject lure;
    public int throw_speed;
    public Camera cam;

    private Transform me_transform;
    private GameObject spawned_lure;

    public void SpawnLure()
    {
        spawned_lure = Instantiate(lure, me_transform.position + new Vector3(0,1,0), lure.transform.rotation);
        spawned_lure.GetComponent<Rigidbody>().velocity += (cam.transform.forward * throw_speed) + new Vector3(0,1*throw_speed,0);

    }

    // Use this for initialization
    void Start () {
        me_transform = this.gameObject.transform;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
