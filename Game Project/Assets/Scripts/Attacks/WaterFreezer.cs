using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFreezer : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other){
		WaterTileObject waterObj = other.GetComponent<WaterTileObject>();
		if (waterObj != null){
			//hit some water, freeze it
			waterObj.Freeze();
		}
	}
}
