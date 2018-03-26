using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyText : MonoBehaviour {

	PlayerControl player;
	Text text;
	int count;

	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag("Player").GetComponent<PlayerControl>();
		text = GetComponent<Text>();
		count = player.hasKeys;
	}
	
	// Update is called once per frame
	void Update () {
		//slightly more expensive when we need to change count; much less expensive all other times
		if (player.hasKeys != count){
			count = player.hasKeys;
			text.text = count.ToString();
		}
		
	}
}
