using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pathfinding;

public class Slime : Actor {
	//navigation fields
	private Navigator navigator;
	public Navigator.BlockingType bType = Navigator.BlockingType.walking;
	private Vector3[] path;

	//obstacle avoidance fields
	private Vector3 avoidVector;
	private Vector2 hitVector;
	int maxHits = 1;
	int numHits;
	private RaycastHit2D[] castHits;
	ContactFilter2D cFilter;
	public Collider2D castCollider;

	//targeting fields
	private GameObject targetObject;
	private Vector3 moveVector;
	public float moveDeadZone = 0.1f;

	// Use this for initialization
	override public void ActorStart () {
		targetObject = GameObject.FindWithTag("Player");
		navigator = GameObject.FindWithTag("Navigator").GetComponent<Navigator>();

		cFilter = Navigator.GetFilterFromBlockingType(bType);
		castHits = new RaycastHit2D[maxHits];
	}
		
	// Update is called once per frame
	void Update () {
		//pathfinding
		path = navigator.GetWorldPath(bType, transform.position, targetObject.transform.position);
		if (path.Length > 0){
			moveVector = (path[0]-transform.position);
		}
		else moveVector = Vector3.zero;

		//avoid local obstacles
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

		//animation
		if (moveVector.magnitude < moveDeadZone){
			moveVector = Vector3.zero;
			animator.SetBool("Walk", false);
		}
		else{
			animator.SetBool("Walk", true);
			sprite.flipX = (moveVector.x > 0); //slime sprite faces left, flip if sprite moving right
		}
	}

	void FixedUpdate(){
		//forces
		//only move if slime is both walking and also not in the middle of a bounce
		if (animator.GetBool("Grounded") == false){
			rbody.AddForce(moveVector * this.maxSpeed);
		}
	}
}
