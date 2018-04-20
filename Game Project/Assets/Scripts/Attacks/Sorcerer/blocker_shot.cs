using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blocker_shot : Projectile {

	public float angVelocity = 6;
	public float flyTime = 0.5f;

	Transform center;
	float theta;
	float pathRadius;
	Vector2 storedVelocity;
	bool circleMode = false;

	void Start(){
	}

	override public void OnInit(){
		center = GameObject.FindObjectOfType<DarkSorceror> ().transform;
		//set initial theta
		theta = Vector2.SignedAngle(Vector2.right, transform.position - center.position);
		storedVelocity = velocity;
		StartCoroutine (FlyTimer ());
	}

	IEnumerator FlyTimer(){
		yield return new WaitForSeconds (flyTime);
		circleMode = true;
		theta = Vector2.SignedAngle(Vector2.right, transform.position - center.position);
		pathRadius = (transform.position - center.position).magnitude;
	}


	//orbit around center object
	override public void FixedUpdate(){
		if (circleMode) {
			Vector3 newPosition = new Vector3 ();
			theta += angVelocity;
			newPosition.x = center.position.x + pathRadius * Mathf.Cos (Mathf.Deg2Rad * theta);
			newPosition.y = center.position.y + pathRadius * Mathf.Sin (Mathf.Deg2Rad * theta);
			transform.position = newPosition;

			if (velocity.Equals (storedVelocity) == false) {
				//we've been reflected; point at center and fly
				velocity = storedVelocity.magnitude * (center.position - transform.position);
				circleMode = false;
			}
		}
		else {
			transform.position += (Vector3)velocity;
		}
	}
}
