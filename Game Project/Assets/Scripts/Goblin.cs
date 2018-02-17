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

	public GameObject marker;
	private GameObject[] markers;
	public LineRenderer line;

	public Navigator.BlockingType bType = Navigator.BlockingType.walking;
	public override void Start(){
		base.Start();
		targetObject = GameObject.FindWithTag("Player");
		navigator = GameObject.FindWithTag("Navigator").GetComponent<Navigator>();

		markers = new GameObject[0];
		line = this.GetComponent<LineRenderer>();

		rbody = this.GetComponent<Rigidbody2D>();
		animator = this.GetComponent<Animator>();
		sprite = this.GetComponent<SpriteRenderer>();
	}

	public void FixedUpdate(){
		rbody.AddForce(Vector3.ClampMagnitude(moveTarget-transform.position, 1f) * maxSpeed);
	}

	public void Update(){
		//chase player
		path = navigator.GetPath(bType, transform.position, targetObject.transform.position);

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




		if (path.Length >= 2)
			moveTarget = path[0]; //not 0, we want to keep moving forward, and will thus move to the second nearest node
	}

}
