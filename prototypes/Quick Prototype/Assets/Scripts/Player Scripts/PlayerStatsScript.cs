using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStatsScript : MonoBehaviour
{
    private Transform HealthBar;
    private Image HealthBarFill;
    private TextMeshProUGUI HealthText;
    private Transform ManaBar;
    private Image ManaBarFill;
    private TextMeshProUGUI ManaText;
    private Transform SheepCount;
    private TextMeshProUGUI SheepText;

    private hellSceneManager hsm;

    void Start() {
        HealthBar = transform.Find("HealthBar");
        HealthBarFill = HealthBar.Find("HealthBarFill").GetComponent<Image>();
        HealthText = HealthBar.Find("Text").GetComponent<TextMeshProUGUI>();
        ManaBar = transform.Find("ManaBar");
        ManaBarFill = ManaBar.Find("ManaBarFill").GetComponent<Image>();
        ManaText = ManaBar.Find("Text").GetComponent<TextMeshProUGUI>();
        SheepCount = transform.Find("SheepCount");
        SheepText = SheepCount.Find("Text").GetComponent<TextMeshProUGUI>();
        
        hsm = FindObjectOfType<hellSceneManager>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = hsm.health;
        float hMax = hsm.healthMax;
        float m = hsm.mana;
        float mMax = hsm.manaMax;
        int sheepCount = hsm.sheepDict.Count;
        HealthBarFill.fillAmount = h / hMax;
        ManaBarFill.fillAmount = m / mMax;
        HealthText.text = string.Format("{0:0}/{1:0}", h, hMax);
        ManaText.text = string.Format("{0:0}/{1:0}", m, mMax);
        SheepText.text = string.Format("Sheep: {0:0}", sheepCount);


    }
}
