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
            
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
