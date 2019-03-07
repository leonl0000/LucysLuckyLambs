using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballScript : MonoBehaviour
{
    public GameObject explosion;
    public float maxFireballDamage = 10; // damage scales with square of fireball power
    public float power; // set when cast, between 0 and 1, denotes charging time
    private const float fireExplosionMaxLight = 4; // maximum intensity of fireball's light
    public const float lifeTimeout = 4f;
    private bool lifeTimerBegun;
    private float lifeTimer;

    // Start is called before the first frame update
    void Start()
    {
        lifeTimer = 0;
        lifeTimerBegun = false;
    }

    // Update is called once per frame
    void Update()
    {
        lifeTimer += lifeTimerBegun? Time.deltaTime : 0;
        if (lifeTimer > lifeTimeout) Destroy(gameObject);
    }

    public void beginLifeTimer() {
        lifeTimerBegun = true;
    }

    public void OnCollisionEnter(Collision collision)
    {
        // Spawn explosion
        GameObject thisExplosion = Instantiate(explosion, this.gameObject.transform.position, this.gameObject.transform.rotation);
        // set explosion scale
        thisExplosion.transform.GetChild(0).gameObject.transform.localScale = new Vector3(power, power, power);
        thisExplosion.transform.GetChild(1).gameObject.transform.localScale = new Vector3(power, power, power);
        // set explosion light intensity
        thisExplosion.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<Light>().intensity = power * fireExplosionMaxLight;

        // And call damage-inflicting function on sheep and wolves, etc.
        if (collision.collider.tag == "sheep")
            collision.gameObject.GetComponent<sheepScript>().wound(maxFireballDamage * power * power, gameObject.transform);
        else if (collision.collider.tag == "Predator")
            collision.gameObject.GetComponent<PredatorScript>().wound(maxFireballDamage * power * power, gameObject.transform);
        else if (collision.collider.tag == "angel")
            collision.gameObject.GetComponent<angelScript>().wound(maxFireballDamage * power * power, gameObject.transform);

        // Destroy fireball
        Destroy(gameObject);
    }
}
