using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEntrancePlayerMover : MonoBehaviour {

	// This is so bad it should be illegal
	void Start () {
		if (GameSaver.liveSave.bossKilled [0]) {
			GameObject.FindObjectOfType<PlayerControl> ().transform.position = transform.position;
		}
	}
}
