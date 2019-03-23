using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellIconScript : MonoBehaviour
{
    public int spellIndex;
    public PlayerMovement pm;
    public Image im;
    void Start()
    {
        spellIndex = Int32.Parse(gameObject.name[2] + "");
        pm = GameObject.Find("Player Container").transform.Find("Player").GetComponent<PlayerMovement>();
        im = gameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        im.fillAmount = 1 - pm.spellCoolDownTimers[spellIndex]/pm.spellCoolDownTimeOut[spellIndex];
    }
}
