using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dagger_shot : Projectile {

	public float initialFlightTime;
	public float homeTime;
	public Transform target;
	public float flySpeedUp;

	private float homingTimer;
	private float baseSpeed;

	override public void OnInit(){
		StartCoroutine(EjectTimer());
	}


	IEnumerator EjectTimer(){
		//fly for a small amount of time in the initial direction
		yield return new WaitForSeconds(initialFlightTime);

		//stop moving for a bit; enable spinning
		baseSpeed = velocity.magnitude;
		velocity = Vector2.zero;
		StartCoroutine(HomingSpin());
	}

	IEnumerator HomingSpin(){
		homingTimer = Time.time;

		while (Time.time <= homingTimer + homeTime){
			transform.up = target.position - transform.position; //look at our target
			yield return null;
		}

		//final mode: fly forward
		velocity = baseSpeed * flySpeedUp * transform.up.normalized;
	}

}
