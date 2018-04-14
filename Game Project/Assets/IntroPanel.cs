using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroPanel : MonoBehaviour {

    public GameObject fireArrow;
	// Use this for initialization
	void Start () {
        if (GameSaver.liveSave.firetutorialpoint)
        {
            gameObject.SetActive(false);
            if (!GameSaver.liveSave.watertutorialpoint)
            {
                fireArrow.SetActive(true);
            }
        }
            
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
