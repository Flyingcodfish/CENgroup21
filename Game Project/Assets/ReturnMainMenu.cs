using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnMainMenu : MonoBehaviour {

    public void Go()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }

    public void SaveGo()
    {
        //save game
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }
}
