using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class circle_shot : Projectile {

	public Vector3 centerPosition;

	public float angVelocity;
	public float pathRadius;

	int sign;
	float theta;

	override public void OnInit(){
		//create center location of path. Center will move in a straight line based on velocity.
		centerPosition = transform.position + (Vector3)(pathRadius * velocity.normalized);
		//set initial theta
		theta = Vector2.SignedAngle(Vector2.right, transform.position - centerPosition);
	}

	//orbit around center object
	override public void FixedUpdate(){
		if (initialized){
			centerPosition += (Vector3)velocity;

			Vector3 newPosition = new Vector3();
			theta += angVelocity;
			newPosition.x = centerPosition.x + pathRadius * Mathf.Cos(Mathf.Deg2Rad * theta);
			newPosition.y = centerPosition.y + pathRadius * Mathf.Sin(Mathf.Deg2Rad * theta);
			transform.position = newPosition;
		}
	}

}
