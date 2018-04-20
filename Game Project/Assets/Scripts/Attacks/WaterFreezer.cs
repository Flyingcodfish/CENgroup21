using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFreezer : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other){
		WaterTileObject waterObj = other.GetComponent<WaterTileObject>();
		if (waterObj != null){
			//hit some water, freeze it
			waterObj.Freeze();
			waterObj.frozenCount += 1; //place a lock on freeze effect until we leave collision
		}
	}


	void OnTriggerExit2D(Collider2D other){
		WaterTileObject waterObj = other.GetComponent<WaterTileObject>();
		if (waterObj != null){
			//hit some water, freeze it
			waterObj.frozenCount -= 1; //release lock on freeze effect
		}
	}

}
