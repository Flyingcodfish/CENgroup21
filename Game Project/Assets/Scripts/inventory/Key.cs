using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other){
		PlayerControl player = other.gameObject.GetComponent<PlayerControl>();
		if (player != null){
			player.hasKeys += 1;
			Destroy(this.gameObject);
		}
	}

}
