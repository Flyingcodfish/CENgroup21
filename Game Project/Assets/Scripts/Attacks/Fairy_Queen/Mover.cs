using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {

	public Vector3 velocity;

	void FixedUpdate(){
		transform.position += velocity;
	}

}
