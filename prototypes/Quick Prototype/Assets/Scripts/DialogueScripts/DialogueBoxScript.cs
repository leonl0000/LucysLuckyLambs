using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DialogueBoxScript : MonoBehaviour
{
    private hellSceneManager hsm;
    public Canvas dialogueCanvas;
    private float delay = -1;
    public List<Page> Pages;
    private Dictionary<string, Page> PageDict;

    private TextMeshProUGUI nameBox;
    private TextMeshProUGUI textBox;
    private RectTransform responseBoxRect;
    private Transform scrollingListTransform;
    private RectTransform scrollingListRect;
    private GameObject scrollBar;
    public static Button ResponseButton;

    private static Vector2 ShortBox = new Vector2(600, 50);
    private static Vector2 TallBox = new Vector2(600, 80);
    private static Vector2 ScrollShortBox = new Vector2(0, 50);
    private List<Button> ResponseButtons;

    void Start()
    {
        hsm = GameObject.Find("GameManager").GetComponent<hellSceneManager>();
        dialogueCanvas = transform.parent.GetComponent<Canvas>();
        nameBox = transform.Find("NameText").GetComponent<TextMeshProUGUI>();
        print(nameBox);
        textBox = transform.Find("TextText").GetComponent<TextMeshProUGUI>();
        responseBoxRect = transform.Find("ResponseBox").GetComponent<RectTransform>();
        scrollingListTransform = responseBoxRect.transform.Find("ScrollingList");
        scrollingListRect = scrollingListTransform.GetComponent<RectTransform>();
        scrollBar = scrollingListTransform.Find("Scrollbar").gameObject;
        ResponseButton = Resources.Load<Button>("ResponseButton");
        ResponseButtons = new List<Button>();

        PageDict = new Dictionary<string, Page>();
        foreach (Page p in Pages)
            PageDict[p.pageName] = p;

        Time.timeScale = 0;
        OpenDialogue();       

    }

    //public void OpenDialogueWithDelay(float delay) {
    //    this.delay = delay;
    //    Debug.Log("Delay Called");
    //}

    public void ExitDialogue() {
        Time.timeScale = 1;
        dialogueCanvas.enabled = false;
    }

    private void GoToPage(Page curPage) {
        nameBox.text = curPage.charName;
        textBox.text = curPage.dialogueText;
        for(int i=ResponseButtons.Count-1; i>=0; i--) {
            Button rb = ResponseButtons[i];
            ResponseButtons.RemoveAt(i);
            Destroy(rb.gameObject);
        }

        //Set Height of response box
        if (curPage.responses.Count < 2)
            responseBoxRect.sizeDelta = ShortBox;
        else responseBoxRect.sizeDelta = TallBox;
        scrollingListRect.offsetMin = -(curPage.responses.Count-1) * ScrollShortBox;
        scrollingListRect.sizeDelta = (curPage.responses.Count-1) * ScrollShortBox;

        for (int i = 0; i < curPage.responses.Count; i++) {
            Button rb = Instantiate(ResponseButton);
            ResponseButtons.Add(rb);
            string nextPage = curPage.responseNextPages[i];
            if (nextPage == "EXIT") rb.onClick.AddListener(() => ExitDialogue());
            else rb.onClick.AddListener(() => GoToPage(PageDict[nextPage]));

            TextMeshProUGUI rbText = rb.transform.Find("Text").GetComponent<TextMeshProUGUI>();
            rbText.text = curPage.responses[i];
            RectTransform rbTransform = rb.GetComponent<RectTransform>();
            rb.transform.SetParent(scrollingListTransform, false);
            rbTransform.anchoredPosition = new Vector2(0, -25 - 50 * i);
        }
    }


    public void OpenDialogue() {
        //dialogueCanvas.gameObject.SetActive(true);
        GoToPage(Pages[0]);
       
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(delay);
        //if (delay > 0) {
        //    delay -= Time.deltaTime;
        //    if (delay <= 0) OpenDialogue();
        //}

    }
}
