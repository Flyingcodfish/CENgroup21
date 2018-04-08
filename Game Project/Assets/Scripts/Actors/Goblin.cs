using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pathfinding;

public class Goblin: AI_Actor {

	//path rendering
	public GameObject marker;
	private GameObject[] markers;
	private LineRenderer pathLine;

	//behavior begins
	public override void AI_Start(){
		markers = new GameObject[0];
		pathLine = this.GetComponent<LineRenderer>();
	}
		
	public void FixedUpdate(){
		rbody.AddForce(moveVector.normalized * maxSpeed);
	}

	override protected void OnTickEnd(){

		//render path lines
		pathLine.positionCount = path.Length;
		pathLine.SetPositions(path);

		//cull node markers
		for (int i=0; i<markers.Length; i++){
			Destroy(markers[i]);
		}

		//make a bunch of node markers
		markers = new GameObject[path.Length];
		for (int i=0; i<path.Length; i++){
			markers[i] = Instantiate(marker, path[i], Quaternion.identity);
		}
	}
}
