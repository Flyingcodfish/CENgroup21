using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trap_shot : Projectile {

	public Vector3 targetPosition;

	public float fadeTime;
	public float circleTime;
	public float angVelocity;
	public float closeSpeed;

	float trapRadius;
	Vector2 storedVelocity;
	float theta;
	bool circleMode = false;
	bool fadeMode = true;
	SpriteRenderer sRenderer;
	Color stepColor;

	override public void OnInit(){
		theta = Vector2.SignedAngle(Vector2.right, transform.position - targetPosition);
		trapRadius = (transform.position - targetPosition).magnitude;
		storedVelocity = velocity;
		StartCoroutine (FadeInTimer ());
	}

	IEnumerator FadeInTimer(){
		float fadeFramerate = 30; //Hz
		float step = 1f / (fadeTime*fadeFramerate);
		sRenderer = GetComponent<SpriteRenderer> ();
		stepColor = sRenderer.color;

		for (float alpha = 0f; alpha < 1f; alpha += step){
			stepColor.a = alpha;
			sRenderer.color = stepColor;
			yield return new WaitForSeconds(1/fadeFramerate);
		}
		fadeMode = false;
		StartCoroutine (CircleTimer ());
	}

	IEnumerator CircleTimer(){
		circleMode = true;
		yield return new WaitForSeconds (circleTime);
		if (circleMode) velocity = closeSpeed * (targetPosition - transform.position).normalized;
		circleMode = false;
	}


	//orbit around target object
	override public void FixedUpdate(){
		if (initialized) {
			if (circleMode || fadeMode) transform.up = targetPosition - transform.position; //point at target
			if (circleMode) {
				Vector3 newPosition = new Vector3 ();
				theta += angVelocity;
				newPosition.x = targetPosition.x + trapRadius * Mathf.Cos (Mathf.Deg2Rad * theta);
				newPosition.y = targetPosition.y + trapRadius * Mathf.Sin (Mathf.Deg2Rad * theta);
				transform.position = newPosition;

				if (velocity.Equals (storedVelocity) == false) {
					//we've been reflected; point away and fly
					velocity = storedVelocity.magnitude * -1 * (targetPosition - transform.position).normalized;
					transform.up = velocity;
					circleMode = false;
				}
			} else if (fadeMode == false) {
				transform.position += (Vector3)velocity;
			}
		}
	}



}
