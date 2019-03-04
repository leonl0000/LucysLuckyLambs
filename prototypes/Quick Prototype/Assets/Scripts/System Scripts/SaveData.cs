using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData 
{
    public float mana;
    public float health;
    public int numSheepDropped;
    public int numSheepEaten;
    public float[] playerPos;


    private float[] Vector3ToFloat(Vector3 v) {
        float[] f = new float[3];
        f[0] = v.x;
        f[1] = v.y;
        f[2] = v.z;
        return f;
    }

    public SaveData(hellSceneManager hsm) {
        mana = hsm.mana;
        health = hsm.health;
        numSheepDropped = hsm.numSheepDropped;
        numSheepEaten = hsm.numSheepEaten;
        playerPos = Vector3ToFloat(hsm.player.transform.position);
    }



}
