using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pathfinding;

public class Goblin: Actor {
	//navigation fields
	private GameObject targetObject;
	private Vector3[] path;
	public Vector3 moveTarget; //DEBUG: field is public
	public Vector3 moveVector; //DEBUG: field is public
	private Navigator navigator;
	public Navigator.BlockingType bType = Navigator.BlockingType.walking;

	//obstacle avoidance fields
	private Vector3 avoidVector;
	private Vector2 hitVector;
	int maxHits = 1;
	int numHits;
	private RaycastHit2D[] castHits;
	ContactFilter2D cFilter;
	public Collider2D castCollider;

	//path rendering
	public GameObject marker;
	private GameObject[] markers;
	private LineRenderer pathLine;

	//behavior begins
	public override void ActorStart(){
		targetObject = GameObject.FindWithTag("Player");
		navigator = GameObject.FindWithTag("Navigator").GetComponent<Navigator>();

		cFilter = Navigator.GetFilterFromBlockingType(bType);
		castHits = new RaycastHit2D[maxHits];

		markers = new GameObject[0];
		pathLine = this.GetComponent<LineRenderer>();
	}
		
	public void FixedUpdate(){
		rbody.AddForce(moveVector.normalized * maxSpeed);
	}

	public void Update(){
		//chase player
		path = navigator.GetWorldPath(bType, transform.position, targetObject.transform.position);
		if (path.Length > 0){
			moveTarget = path[0]; 
		}
			
		//avoid local obstacles
		moveVector = moveTarget - transform.position;
		numHits = castCollider.Cast((Vector2)moveVector, cFilter, castHits, moveVector.magnitude);
		//for now we only care about 1st hit
		if (numHits > 0){
			//add a force that is perpendicular to the path we take to hit an obstacle.
			//This moves simultaneously away from the obstacle, and towards our goal.
			hitVector = castHits[0].point - ((Vector2)transform.position + castCollider.offset);
			float phi = Vector2.SignedAngle(moveVector, hitVector);
			float sign = Mathf.Sign(phi); //choose which of two perpendicular paths to take
			avoidVector = new Vector2(sign*hitVector.y, -1*sign*hitVector.x); //calculate normal to hitVector
			moveVector = avoidVector.normalized + moveVector.normalized;
		}

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
