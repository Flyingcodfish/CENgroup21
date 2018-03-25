using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilemapFollow : MonoBehaviour {

	private Transform follow;

	void Start(){
		follow = GameObject.FindWithTag("Player").transform;
	}

	// Update is called once per frame
	void Update () {
		this.transform.position = new Vector2(Mathf.Round(follow.position.x), Mathf.Round(follow.position.y));
	}
}
