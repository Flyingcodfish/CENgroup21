using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroPanel : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (GameSaver.liveSave.firetutorialpoint)
        {
            gameObject.SetActive(false);
        }

		if (GameSaver.liveSave.hasBeenNamed) {
			//we already named the player. don't show this panel.
			gameObject.SetActive (false);
		}
		else { //the player has not yet been named; turn them off until we name them
			GameObject.FindObjectOfType<PlayerControl>().enabledAI = false;
		}
	}
}
