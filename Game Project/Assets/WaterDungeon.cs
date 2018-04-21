using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDungeon : MonoBehaviour {
    public GameObject introMsg;
	// Use this for initialization
	void Start () {
        if (GameSaver.liveSave.firstIce) introMsg.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
