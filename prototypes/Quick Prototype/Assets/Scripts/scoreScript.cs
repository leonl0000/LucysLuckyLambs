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
        if (hsm.health > 0)
            scoreText.text = string.Format("Health: {0:0}\nMana: {1}\nFallen: {2}\nEaten: {3}",
                hsm.health, hsm.mana, hsm.numSheepDropped, hsm.numSheepEaten);
        else scoreText.text = "YOU ARE DEAD\nDEAD\nDEAD\nDEAD";
    }
}
