using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NesScripts.Controls.PathFind;

public class Goblin : Actor {
	private GameObject targetObject;
	private Vector3[] path;
	private Vector3 moveTarget;
	private Navigator navigator;


	public float maxSpeed;
	private Rigidbody2D rbody;
	private Animator animator;
	private SpriteRenderer sprite;


	public Navigator.BlockingType bType = Navigator.BlockingType.walking;
	public override void Start(){
		base.Start();
		targetObject = GameObject.FindWithTag("Player");
		navigator = GameObject.FindWithTag("Navigator").GetComponent<Navigator>();

		rbody = this.GetComponent<Rigidbody2D>();
		animator = this.GetComponent<Animator>();
		sprite = this.GetComponent<SpriteRenderer>();
	}


	public void Update(){
		//chase player
		path = navigator.GetPath(bType, transform.position, targetObject.transform.position);
		if (path.Length >= 2)
			moveTarget = path[1]; //not 0, we want to keep moving forward, and will thus move to the second nearest node
		rbody.AddForce(Vector3.MoveTowards(transform.position, moveTarget, 1f) * maxSpeed);
	}

}
