using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class angelBoltScript : MonoBehaviour
{

    public float boltDamage = 5;
    private hellSceneManager hsm;

    // Start is called before the first frame update
    void Start()
    {
        hsm = FindObjectOfType<hellSceneManager>();
    }

    public void OnCollisionEnter(Collision collision)
    {
        // Spawn explosion
        //GameObject thisExplosion = Instantiate(explosion, this.gameObject.transform.position, this.gameObject.transform.rotation);
        // TODO make bolt explosion and instantiate above?

        // And call damage-inflicting function on sheep and wolves, etc.

        HealthScript healthScript = collision.collider.GetComponent<HealthScript>();
        if (healthScript != null) healthScript.wound(boltDamage, gameObject.transform);

        else if (collision.collider.tag == "Player") {
            hsm.health -= 5;
            if (hsm.health < 0) Destroy(hsm.player);
        }

        // Destroy bolt
        Destroy(gameObject);
    }
}
