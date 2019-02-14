using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilities : MonoBehaviour {

    public hellSceneManager hsm;

    public GameObject lure;
    public int throw_speed;
    public Camera cam;

    private Transform me_transform;
    private GameObject spawned_lure;

    private int nextIndex = 0;

    public void SpawnLure()
    {
        if (hsm.mana >= 2) {
            hsm.mana -= 2;
            spawned_lure = Instantiate(lure, me_transform.position + new Vector3(0, 1, 0), lure.transform.rotation);
            spawned_lure.GetComponent<Rigidbody>().velocity += (cam.transform.forward * throw_speed) + new Vector3(0, 1 * throw_speed, 0);

            hsm.lureDict[nextIndex] = spawned_lure;
            spawned_lure.GetComponent<LureScript>().index = nextIndex;
            nextIndex++;
        }
    }

    // Use this for initialization
    void Start () {
        me_transform = this.gameObject.transform;
	}

	// Update is called once per frame
	void Update () {

	}
}
