using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject letter;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void loadLevelOne() {
        SceneManager.LoadScene("Level 01");
    }

    public void openLetter() {
        letter.active = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
