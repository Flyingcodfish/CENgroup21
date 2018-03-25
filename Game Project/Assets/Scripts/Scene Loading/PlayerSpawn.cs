using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour {

	public PlayerControl playerPrefab;
	private GameObject playerInstance;

	public bool isDefaultSpawnPoint = true; //when there are multiple spawn points in a scene, default to this one if none were explicitly chosen by level loader

	// Use this for initialization
	void Awake () {
		playerInstance = GameObject.FindWithTag("Player");
		//spawn player if none present
		if ( playerInstance == null){
			Instantiate(playerPrefab, transform.position, Quaternion.identity);
		}
		//else spawn player at this location
		//TODO: interface with level loader to make scene loads choose a spawn point to teleport the player to.
		else{
			playerInstance.transform.position = this.transform.position;
		}

		//turn off preview sprite
		this.GetComponent<SpriteRenderer>().enabled = false;

	}
}
