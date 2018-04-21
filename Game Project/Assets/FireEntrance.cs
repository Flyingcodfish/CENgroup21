using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEntrance : MonoBehaviour {
    public GameObject arrow;
    public GameObject arrow2;
	// Use this for initialization
	void Start () {
		if (GameSaver.liveSave.bossKilled[0])
        {
            arrow.SetActive(false);
            arrow2.SetActive(false);
        } 
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
