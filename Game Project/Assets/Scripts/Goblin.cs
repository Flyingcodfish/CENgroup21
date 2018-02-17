using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NesScripts.Controls.PathFind;

public class Goblin : Actor {
	//navigation fields
	private GameObject targetObject;
	private Vector3[] path;
	private Vector3 moveTarget;
	private Navigator navigator;
	public Navigator.BlockingType bType = Navigator.BlockingType.walking;

	//path rendering
	public GameObject marker;
	private GameObject[] markers;
	private LineRenderer line;

	//behavior begins
	public override void ActorStart(){
		targetObject = GameObject.FindWithTag("Player");
		navigator = GameObject.FindWithTag("Navigator").GetComponent<Navigator>();

		markers = new GameObject[0];
		line = this.GetComponent<LineRenderer>();
	}

	public void FixedUpdate(){

		rbody.AddForce((moveTarget-transform.position).normalized * maxSpeed);
	}

	public void Update(){
		//chase player
		path = navigator.GetPath(bType, transform.position, targetObject.transform.position);
		if (path.Length > 0){
			moveTarget = path[0]; 
		}

		//render path lines
		line.positionCount = path.Length;
		line.SetPositions(path);

		//cull markers
		for (int i=0; i<markers.Length; i++){
			Destroy(markers[i]);
		}

		//make a bunch of markers
		markers = new GameObject[path.Length];
		for (int i=0; i<path.Length; i++){
			markers[i] = Instantiate(marker, path[i], Quaternion.identity);
		}

	}

}
