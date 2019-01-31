using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scoreScript : MonoBehaviour
{
    public hellSceneManager hsm;
    public Text scoreText;

    // Update is called once per frame
    void Update()    {
        scoreText.text = string.Format("Mana: {0}\nFallen: {1}", hsm.mana, hsm.numSheepDropped);
    }
}
