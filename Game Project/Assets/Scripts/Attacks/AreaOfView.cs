using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class AreaOfView : MonoBehaviour {


	private Actor parentActor;
	private AI_Actor enemyScript;
	private Team parentTeam;
	// Use this for initialization
	void Start () {
		this.parentActor = this.GetComponentInParent<Actor>();
		this.parentTeam = this.GetComponentInParent<TeamComponent>().team;
		enemyScript = parentActor.GetComponent<AI_Actor>();

	}

	public void OnTriggerEnter2D(Collider2D other) {
		TeamComponent otherTeam = other.gameObject.GetComponent<TeamComponent>();
		if (otherTeam.team != this.parentTeam){
			enemyScript.enabledAI = true;

		}


	}

	public void OnTriggerExit2D(Collider2D other) {
		TeamComponent otherTeam = other.gameObject.GetComponent<TeamComponent>();
		if (otherTeam.team != this.parentTeam){
			enemyScript.enabledAI = false;
			enemyScript.moveVector = Vector3.zero;
		}

	}
}
