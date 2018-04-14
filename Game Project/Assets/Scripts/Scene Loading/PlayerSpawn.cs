using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour {

	public GameObject playerPrefab;
	private GameObject playerInstance;

	public Material sceneMaterial;

	public bool isDefaultSpawnPoint = true; //TODO: field for when there are multiple spawnPoints in a scene, for scene transitions or loading a saved game in a certain location

	// Use this for initialization
	void Awake () {
		//creates a live SavedGame if none is present, only used when launching the game from NOT the main menu
		if (GameSaver.liveSave == null) {
			GameSaver.liveSave = new SavedGame ();
		}

		playerInstance = GameObject.FindWithTag("Player");
		//spawn player if none present
		if ( playerInstance == null){
			playerInstance = Instantiate(playerPrefab, transform.position, Quaternion.identity);
		}
		//else spawn player at this location
		//TODO: interface with level loader to make scene loads choose a spawn point to teleport the player to.
		else{
			playerInstance.transform.position = this.transform.position;
		}

		playerInstance.GetComponent<SpriteRenderer> ().material = sceneMaterial; //allows scenes to use dynamic lighting

		//turn off preview sprite
		this.GetComponent<SpriteRenderer>().enabled = false;

	}
}
