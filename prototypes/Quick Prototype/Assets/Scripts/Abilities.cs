﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilities : MonoBehaviour {

    public hellSceneManager hsm;

    public GameObject lure;
    public int throw_speed;
    private GameObject spawned_lure;

    public GameObject fireball;
    public float minFireballSpeed = 40;
    public float maxFireballSpeed = 100;
    private GameObject spawned_fireball;
    public bool isGrowingFireball = false;
    private float fireballGrowLength;
    private const float fireballMinPower = 0.2f;
    private const float fireballAimSphereMaxScale = 0.4f;
    private const float fireballParticlesMaxScale = 2;
    private const float fireballMaxGrowLength = 3; // length in seconds to grow fireball to full size
    private const float fireballMaxLight = 3; // maximum intensity of fireball's light

    public Camera cam;

    private Transform me_transform;

    private int nextIndex = 0;

    void FixedUpdate()
    {
        // move currently-growing fireball to new position
        // done in FixedUpdate so it doesn't lag behind player movement
        if (isGrowingFireball)
            spawned_fireball.transform.position = cam.transform.position + cam.transform.forward * 10;
    }

    public void SpawnLure()
    {
        if (hsm.mana >= 2) {
            hsm.mana -= 2;
            spawned_lure = Instantiate(lure, me_transform.position + new Vector3(0, 13, 0), lure.transform.rotation);
            //spawned_lure.GetComponent<Rigidbody>().velocity += (cam.transform.forward * throw_speed) + new Vector3(0, 1 * throw_speed, 0);

            hsm.lureDict[nextIndex] = spawned_lure;
            spawned_lure.GetComponent<LureScript>().index = nextIndex;
            nextIndex++;
        }
    }

    /* Called when fireball key is held down. Can either spawn or grow fireball */
    public void FireballKey()
    {
        // possibly create a new fireball
        if (!isGrowingFireball && hsm.mana >= 2)
        {
            isGrowingFireball = true;
            fireballGrowLength = 0;
            hsm.mana -= 2;
            spawned_fireball = Instantiate(fireball, cam.transform.position + cam.transform.forward * 10, fireball.transform.rotation);

            // deactivate particles and light while aiming
            spawned_fireball.gameObject.transform.GetChild(0).gameObject.GetComponent<Renderer>().enabled = false;

            // set fireball aiming sphere's scale
            float currFireballScale = fireballMinPower * fireballAimSphereMaxScale;
            spawned_fireball.gameObject.transform.localScale = new Vector3(currFireballScale, currFireballScale, currFireballScale);
        }
        // otherwise, grow current fireball
        else if (isGrowingFireball)
        {
            // calculate current power
            fireballGrowLength += Time.deltaTime;
            float growFraction = Mathf.Max(Mathf.Min(1, fireballGrowLength / fireballMaxGrowLength), fireballMinPower);
            float currPower = fireballMinPower + growFraction * (1 - fireballMinPower);

            // set fireball scale according to power
            float currFireballScale = currPower * fireballAimSphereMaxScale;
            spawned_fireball.gameObject.transform.localScale = new Vector3(currFireballScale, currFireballScale, currFireballScale);

            // set light intensity according to power
            spawned_fireball.gameObject.transform.GetChild(1).gameObject.GetComponent<Light>().intensity = currPower * fireballMaxLight;
        }
    }

    public void FireballRelease()
    {
        // compute fireball power
        isGrowingFireball = false;
        float growFraction = Mathf.Min(1, fireballGrowLength / fireballMaxGrowLength);
        float currPower = fireballMinPower + growFraction * (1 - fireballMinPower);
        spawned_fireball.GetComponent<FireballScript>().power = currPower;

        // activate particles, and set to right scale
        spawned_fireball.gameObject.transform.GetChild(0).gameObject.GetComponent<Renderer>().enabled = true;
        float currParticleScale = currPower * fireballParticlesMaxScale;
        spawned_fireball.transform.GetChild(0).gameObject.transform.localScale = new Vector3(currParticleScale, currParticleScale, currParticleScale);

        // deactivate the aiming sphere
        spawned_fireball.gameObject.GetComponent<Renderer>().enabled = false;

        // shoot fireball forward with appropriate speed, smaller ones shoot faster
        float fireballSpeed = minFireballSpeed + (1 - currPower) * (maxFireballSpeed - minFireballSpeed);
        spawned_fireball.GetComponent<Rigidbody>().velocity += cam.transform.forward * fireballSpeed;
    }

    // Use this for initialization
    void Start () {
        me_transform = this.gameObject.transform;
	}

	// Update is called once per frame
	void Update () {

	}
}
