using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	GameObject player;
	LevelLoader loader;

	void Start (){
		loader = FindObjectOfType<LevelLoader> ();
		player = GameObject.FindWithTag("Player");
		Destroy (player);
	}

    public void NewGame()
    {
		Debug.Log ("Starting new game.");
		GameSaver.liveSave = new SavedGame(); //resets current saved data
		loader.LoadLevel(SceneUtility.GetBuildIndexByScenePath("test")); //fight me
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
