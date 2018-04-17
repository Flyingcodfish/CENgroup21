using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class circle_shot : Projectile {

	public Mover centerPrefab;
	private Mover centerInstance;

	public float angVelocity;
	public float pathRadius;

	int sign;
	float theta;

	override public void OnInit(){
		//create center object for path. Will move in a straight line
		Vector3 centerPosition = transform.position + (Vector3)(pathRadius * velocity.normalized);
		centerInstance = Instantiate (centerPrefab, centerPosition, Quaternion.identity);
		centerInstance.velocity = velocity; //center instance moves towards player
		transform.SetParent(centerInstance.transform, true); //follow this object

		//set initial theta
		theta = Vector2.SignedAngle(Vector2.right, transform.localPosition);
	}

	//orbit around center object
	override public void FixedUpdate(){
		if (initialized){
			Vector3 newPosition = new Vector3();
			theta += angVelocity;
			newPosition.x = centerInstance.transform.position.x + pathRadius * Mathf.Cos(Mathf.Deg2Rad * theta);
			newPosition.y = centerInstance.transform.position.y + pathRadius * Mathf.Sin(Mathf.Deg2Rad * theta);
			transform.position = newPosition;
		}
	}

	void OnDestroy(){
		Destroy(centerInstance.gameObject);
	}

}
