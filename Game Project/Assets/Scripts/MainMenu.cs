using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	GameObject player;

	void Start (){
		player = GameObject.FindWithTag("Player");
		if (player != null) player.SetActive(false);
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
		SceneManager.LoadScene("enemy_test");
	}

    public void QuitGame()
    {
        Application.Quit();
    }
}
