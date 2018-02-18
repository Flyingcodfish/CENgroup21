using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pathfinding;

public class Slime : Actor {
	//navigation fields
	private Navigator navigator;
	public Navigator.BlockingType bType = Navigator.BlockingType.walking;
	private Vector3[] path;

	//targeting fields
	private GameObject targetObject;
	private Vector3 moveVector;
	public float moveDeadZone = 0.1f;

	// Use this for initialization
	override public void ActorStart () {
		targetObject = GameObject.FindWithTag("Player");
		navigator = GameObject.FindWithTag("Navigator").GetComponent<Navigator>();
	}
		
	// Update is called once per frame
	void Update () {
		//pathfinding
		path = navigator.GetWorldPath(bType, transform.position, targetObject.transform.position);
		if (path.Length > 0){
			moveVector = (path[0]-transform.position);
		}
		else moveVector = Vector3.zero;



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
