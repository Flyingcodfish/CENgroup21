using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class AreaOfView : MonoBehaviour {

	public float aggroRadius;
	public float de_aggroRadius;

	private Actor parentActor;
	private AI_Actor enemyScript;
	private Team parentTeam;
	private CircleCollider2D trigger;

	// Use this for initialization
	void Start () {
		this.parentActor = this.GetComponentInParent<Actor>();
		this.parentTeam = this.GetComponentInParent<TeamComponent>().team;
		enemyScript = parentActor.GetComponent<AI_Actor>();
		trigger = GetComponent<CircleCollider2D> ();
		//defaults to use trigger radius, and for two ranges to be the same
		//means no work is required to update existing enemies, they'll behave as before
		if (aggroRadius == 0f)
			aggroRadius = trigger.radius;
		if (de_aggroRadius == 0f)
			de_aggroRadius = aggroRadius;

		trigger.radius = aggroRadius;
	}

	public void OnTriggerEnter2D(Collider2D other) {
		TeamComponent otherTeam = other.gameObject.GetComponent<TeamComponent>();
		if (otherTeam.team != this.parentTeam){
			enemyScript.lockAI -= 1;
			trigger.radius = de_aggroRadius;
		}


	}

	public void OnTriggerExit2D(Collider2D other) {
		TeamComponent otherTeam = other.gameObject.GetComponent<TeamComponent>();
		if (otherTeam.team != this.parentTeam){
			enemyScript.lockAI += 1;
			enemyScript.moveVector = Vector3.zero;
			trigger.radius = aggroRadius;
		}

	}
}
