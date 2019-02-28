using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballScript : MonoBehaviour
{
    public GameObject explosion;
    public float maxFireballDamage = 10;
    public float power; // set when cast, between 0 and 1, denotes charging time

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        // Spawn explosion
        GameObject thisExplosion = Instantiate(explosion, this.gameObject.transform.position, this.gameObject.transform.rotation);
        thisExplosion.transform.GetChild(0).gameObject.transform.localScale = new Vector3(power, power, power);

        // And call damage-inflicting function on sheep and wolves, etc.
        if (collision.collider.tag == "sheep")
            collision.gameObject.GetComponent<sheepScript>().wound(maxFireballDamage * power, gameObject.transform);
        else if (collision.collider.tag == "Predator")
            collision.gameObject.GetComponent<PreditorScript>().wound(maxFireballDamage * power, gameObject.transform);

        // Destroy fireball
        Destroy(this.gameObject);
    }
}
