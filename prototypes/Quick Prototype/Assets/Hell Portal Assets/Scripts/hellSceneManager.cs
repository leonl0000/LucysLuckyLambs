using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hellSceneManager : MonoBehaviour {
    public float mana;
    public int numSheepDropped;
    public GameObject sheep;
    public Dictionary<int, GameObject> sheepDict;
    private int nextIndex;

    public void Start() {
        mana = 50;
        numSheepDropped = 0;
        sheepDict = new Dictionary<int, GameObject>();
        nextIndex = 0;
    }


    //HELLGATE Objects
    public void objectEnterHellgate(GameObject o, hellgateScript hellGate) {
        if (o.tag == "sheep") {
            Destroy(o);
            sheepDict.Remove(o.GetComponent<sheepScript>().index);
            //Debug.Log(string.Format("Sheep {0} SACRIFICED", o.GetComponent<sheepScript>().index));
            mana += 100;
            hellGate.numSacrificed += 1;
        }
    }

    //Deal with objects that fall off the edge
    public void objectDrop(GameObject o) {
        if (o.tag == "sheep") {
            Destroy(o);
            sheepDict.Remove(o.GetComponent<sheepScript>().index);
            //Debug.Log(string.Format("Sheep {0} dropped", o.GetComponent<sheepScript>().index));
            numSheepDropped += 1;
        }
    }

    public bool spawnSheepAt(Vector3 pos) {
        if (mana > 0) {
            mana--;
            GameObject s = Instantiate(sheep, pos, sheep.transform.rotation);
            s.GetComponent<sheepScript>().index = nextIndex;
            sheepDict[nextIndex] = s;
            nextIndex++;
            //Debug.Log(string.Format("Sheep {0} Born", s.GetComponent<sheepScript>().index));
            return true;
        }
        return false;
    }

}
