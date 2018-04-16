using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFreezer : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other){
		WaterTileObject waterObj = other.GetComponent<WaterTileObject>();
		Fountain fountain = other.GetComponent<Fountain>();
		if (waterObj != null){
			//hit some water, freeze it
			waterObj.Freeze();
		}
	}

}
