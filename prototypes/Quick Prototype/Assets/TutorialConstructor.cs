using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class TutorialConstructor : MonoBehaviour
{
    // Start is called before the first frame update
    private TextMeshProUGUI TutorialText;
    private Button Button;
    private TextMeshProUGUI ButtonText;
    private RectTransform PanelDims;

    public void Start()
    {
        TutorialText = gameObject.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        Button = gameObject.transform.Find("Button").GetComponent<Button>();
        ButtonText = Button.transform.Find("text").GetComponent<TextMeshProUGUI>();
        PanelDims = gameObject.GetComponent<RectTransform>();
        Debug.Log(PanelDims);
    }

    public void Build(string text, string buttonText, Action buttonFunction, 
        int xPos=0, int yPos=0, float scaleWidth=.8f, float scaleHeight = .8f
        ) {
        PanelDims.localPosition = new Vector2(xPos, yPos);
        PanelDims.localScale = new Vector2(scaleWidth, scaleHeight);
        TutorialText.text = text;
        ButtonText.text = buttonText;
        Button.onClick.AddListener(delegate { buttonFunction(); });


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
