using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuScript : MonoBehaviour
{
    public static bool GameIsPaused = false;
    private GameObject PauseMenu;
    private GameObject LoadButton1;
    private GameObject LoadButton2;
    private GameObject LoadButton3;

    void Start() {
        PauseMenu = gameObject.transform.Find("PauseMenu").gameObject;
        if (PauseMenu == null) Debug.LogError("Pause Menu Not Found");
        LoadButton1 = PauseMenu.transform.Find("GameSlot1").Find("loadButton").gameObject;
        LoadButton2 = PauseMenu.transform.Find("GameSlot2").Find("loadButton").gameObject;
        LoadButton3 = PauseMenu.transform.Find("GameSlot3").Find("loadButton").gameObject;

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            if (GameIsPaused) Resume();
            else Pause();
        }
        if(GameIsPaused) {
        LoadButton1.SetActive(SaveSystem.SaveFileExists(1));
        LoadButton2.SetActive(SaveSystem.SaveFileExists(2));
        LoadButton3.SetActive(SaveSystem.SaveFileExists(3));
        }
    }

    public void Resume() {
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause() {
        PauseMenu.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        LoadButton1.SetActive(SaveSystem.SaveFileExists(1));
        LoadButton2.SetActive(SaveSystem.SaveFileExists(2));
        LoadButton3.SetActive(SaveSystem.SaveFileExists(3));
    }
}
