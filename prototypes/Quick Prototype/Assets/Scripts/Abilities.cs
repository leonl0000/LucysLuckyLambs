using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilities : MonoBehaviour {

    public hellSceneManager hsm;

    public GameObject lure;
    public int throw_speed;
    private GameObject spawned_lure;

    public GameObject fireball;
    public int fireball_speed;
    private GameObject spawned_fireball;
    public bool isGrowingFireball = false;
    private float fireballGrowLength;
    private const float fireballMinPower = 0.1f;
    private const float fireballMaxScale = 0.2f;
    private const float fireballParticlesMaxScale = 2;
    private const float fireballMaxGrowLength = 3; // length in seconds to grow fireball to full size

    public Camera cam;

    private Transform me_transform;

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

    /* Called when fireball key is held down. Can either spawn or grow fireball */
    public void FireballKey()
    {
        // possibly create a new fireball
        if (!isGrowingFireball && hsm.mana >= 2)
        {
            isGrowingFireball = true;
            fireballGrowLength = 0;
            hsm.mana -= 2;
            spawned_fireball = Instantiate(fireball, me_transform.position + new Vector3(0, 3, 0), fireball.transform.rotation);
            float currFireballScale = fireballMinPower * fireballMaxScale;
            spawned_fireball.gameObject.transform.localScale = new Vector3(currFireballScale, currFireballScale, currFireballScale);
            float currParticleScale = fireballMinPower * fireballParticlesMaxScale;
            spawned_fireball.transform.GetChild(0).gameObject.transform.localScale = new Vector3(currParticleScale, currParticleScale, currParticleScale);
        }
        // otherwise, grow current fireball
        else if (isGrowingFireball)
        {
            fireballGrowLength += Time.deltaTime;
            float growFraction = Mathf.Max(Mathf.Min(1, fireballGrowLength / fireballMaxGrowLength), fireballMinPower);
            float currFireballScale = growFraction * fireballMaxScale;
            spawned_fireball.gameObject.transform.localScale = new Vector3(currFireballScale, currFireballScale, currFireballScale);
            float currParticleScale = growFraction * fireballParticlesMaxScale;
            spawned_fireball.transform.GetChild(0).gameObject.transform.localScale = new Vector3(currParticleScale, currParticleScale, currParticleScale);
        }
    }

    public void FireballRelease()
    {
        spawned_fireball.GetComponent<Rigidbody>().velocity += me_transform.forward * fireball_speed;
        isGrowingFireball = false;
        float growFraction = Mathf.Min(1, fireballGrowLength / fireballMaxGrowLength);
        spawned_fireball.GetComponent<FireballScript>().power = growFraction;
    }

    // Use this for initialization
    void Start () {
        me_transform = this.gameObject.transform;
	}

	// Update is called once per frame
	void Update () {

	}
}
