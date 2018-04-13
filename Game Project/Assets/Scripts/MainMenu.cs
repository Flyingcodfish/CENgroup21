using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	GameObject player;

	void Start (){
		player = GameObject.FindWithTag("Player");
		Destroy (player);
		if (GameSaver.gameSaverInstance == null) new GameSaver (); //creates new GameSaver. A global field now exists that can acess it from anywhere: GameSaver.gameSaverInstance.
	}

    public void NewGame()
    {
		GameSaver.gameSaverInstance.liveSave = new SavedGame (); //resets current saved data
		SceneManager.LoadScene("test");
    }

	public void LoadGame()
	{
		GameSaver.LoadGame (); //reads save from file into live save
		SceneManager.LoadScene(GameSaver.gameSaverInstance.liveSave.sceneName);
	}

    public void QuitGame()
    {
        Application.Quit();
    }
}
