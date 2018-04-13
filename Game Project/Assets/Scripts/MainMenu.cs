using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	GameObject player;

	void Start (){
		player = GameObject.FindWithTag("Player");
		Destroy (player);
	}

    public void NewGame()
    {
		GameSaver.liveSave = new SavedGame (); //resets current saved data
		SceneManager.LoadScene("test");
    }

	public void LoadGame()
	{
		GameSaver.LoadGame (); //reads save from file into live save
		SceneManager.LoadScene(GameSaver.liveSave.sceneName);
	}

    public void QuitGame()
    {
        Application.Quit();
    }
}
