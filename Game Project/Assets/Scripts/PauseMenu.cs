using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject settingsMenuUI;
    public GameObject saveGameUI;
    public GameObject QuitUI;
    public GameObject ReturnMainUI;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                inventory.HudGroup.alpha = 1;
                Resume();
            }
            else
            {
                inventory.HudGroup.alpha = 0;
                Pause();
            }
        }
	}

    public void Resume()
    {
        if (pauseMenuUI.activeSelf == true)
        {
            pauseMenuUI.SetActive(false);
        }
        else if (settingsMenuUI.activeSelf == true)
        {
            settingsMenuUI.SetActive(false);
        }

        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        //if saved
        //Time.timeScale = 1f;
        //SceneManager.LoadScene("Main Menu");

        //if not saved
        pauseMenuUI.SetActive(false);
        ReturnMainUI.SetActive(true);

    }

    public void SaveGame()
    {
        pauseMenuUI.SetActive(false);
        saveGameUI.SetActive(true);
        //save game code
    }

    public void QuitGame()
    {
        //if saved
        //Application.Quit();

        //if not saved
        pauseMenuUI.SetActive(false);
        QuitUI.SetActive(true);
    }
}
