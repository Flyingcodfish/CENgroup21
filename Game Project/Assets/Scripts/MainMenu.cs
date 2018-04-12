using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	GameObject player;

	void Start (){
		player = GameObject.FindWithTag("Player");
		if (player != null) player.SetActive(false);
		new GameSaver (); //creates new GameSaver. A global field now exists that can acess it from anywhere: GameSaver.gameSaverInstance.
	}

    public void NewGame()
    {
		if (player != null){
			player.SetActive(true);
			DestroyImmediate(player); //TODO does not actually delete player
		}
        SceneManager.LoadScene("test");    
    }

	public void LoadGame()
	{
		if (player != null) player.SetActive(true);

		//TODO: actually load a saved game from disk, and load a scene depending on the player's last location.
		SceneManager.LoadScene("enemy_test");
	}

    public void QuitGame()
    {
        Application.Quit();
    }
}
