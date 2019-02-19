using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject loadMenu;
    private GameObject canvas;
    private static string LuciferLetter = "Heya!\n" +
        "I'm going overseas for a bit. Would you kindly look after my sheep" +
        " while I'm gone?\n\n\n" +
        "~ Luci <3";





    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas");
    }

    public void loadLevelOne() {
        SceneManager.LoadScene("Level 01");
    }

    public void loadSave(int slot) {
        SaveSystem.saveSlot = slot;
        loadLevelOne();
    }

    public void openLetter() {
        GameObject Letter = Instantiate(Resources.Load("TutorialItem")) as GameObject;
        Letter.transform.SetParent(canvas.transform, false);
        Letter.GetComponent<TutorialConstructor>().Start(); 
        Letter.GetComponent<TutorialConstructor>().Build(LuciferLetter, "Sure!", loadLevelOne);
        Letter.SetActive(true);
    }

    public void closeLoadMenu() {
        GameObject.Find("LoadMenu").SetActive(false);
    }

    public void openLoadMenu() {
        loadMenu.SetActive(true);
        loadMenu.transform.Find("GameSlot1").Find("loadButton").gameObject.SetActive(SaveSystem.SaveFileExists(1));
        loadMenu.transform.Find("GameSlot2").Find("loadButton").gameObject.SetActive(SaveSystem.SaveFileExists(2));
        loadMenu.transform.Find("GameSlot3").Find("loadButton").gameObject.SetActive(SaveSystem.SaveFileExists(3));



    }

}
