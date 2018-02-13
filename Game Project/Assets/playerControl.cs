using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerControl : MonoBehaviour {

	public float maxSpeed;

	private Rigidbody2D rbody;
	private Vector2 input;

	void Start (){
		rbody = gameObject.GetComponent<Rigidbody2D>();
	}
		
	void FixedUpdate () {
		input.x = Input.GetAxisRaw("Horizontal") * maxSpeed;
		input.y = Input.GetAxisRaw("Vertical") * maxSpeed;

		rbody.MovePosition((Vector2)transform.position + input);
	}
}
