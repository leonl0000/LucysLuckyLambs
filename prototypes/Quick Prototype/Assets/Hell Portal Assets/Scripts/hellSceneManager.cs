using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hellSceneManager : MonoBehaviour
{
    public float mana;
    public int numSheepDropped;

    public void Start() {
        mana = 50;
        numSheepDropped = 0;
    }

    //HELLGATE Objects
    public void objectEnterHellgate(GameObject o, hellgateScript hellGate) {
        if(o.tag == "sheep") {
            Destroy(o);
            mana += 100;
            hellGate.numSacrificed += 1;
        }
    }

    //Deal with objects that fall off the edge
    public void objectDrop(GameObject o) {
        if(o.tag == "sheep") {
            Destroy(o);
            numSheepDropped += 1;
        }
    }

}
