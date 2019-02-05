using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hellgateScript : MonoBehaviour {

    public hellSceneManager hsm;
    public int numSacrificed = 0;

    //Everything passed to scene manager
    private void OnTriggerEnter(Collider other) {
        hsm.objectEnterHellgate(other.gameObject, this);
    }


}
