using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkSorceror : AI_Actor {

	public GameObject teleportExit;
	public GameObject deathFlashPrefab;

	public Transform fireTarget;
	private Vector3 fireDirection;

	private float attackTimer;
	private float attackCooldown;

	private float teleportTimer;
	private float teleportTime; //maybe a pair of values representing a range


	//attack fields
	public circle_shot circleShotPrefab;
	private circle_shot circleShotInstance;
	public float circleSpeed = 0.1f;
	public float circleShotInterval = 0.15f;
	public int circleShotCount = 3;

	public Projectile spreadShotPrefab;
	private Projectile spreadShotInstance;
	public float spreadShotInterval = 0.25f;
	public float spreadRange = 90;
	public int spreadWaveCount = 3;
	public int spreadCount = 5;
	public float spreadSpeed = 0.1f;

	public blocker_shot blockerShotPrefab;
	private blocker_shot blockerShotInstance;
	public float blockerInitialSpeed = 0.1f;
	public int blockerCount = 12;

	public trap_shot trapShotPrefab;
	private trap_shot trapShotInstance;
	public int trapCount = 8;
	public float trapInitialSpeed = 0.15f;
	public float trapRadius = 2f;

	//
	//BEHAVIOR BEGINS
	//

	void Update(){
		//test fire
		if (Input.GetKeyDown(KeyCode.F)){
			FireCircleWave();
		}
		if (Input.GetKeyDown(KeyCode.G)){
			FireSpreadWave();
		}
		if (Input.GetKeyDown(KeyCode.H)){
			StartBlocking();
		}
		if (Input.GetKeyDown (KeyCode.J)) {
			FireTrap ();
		}
	}




	override public void AI_Start(){
		if (GameSaver.liveSave.bossKilled [2] == true)
			Destroy (this.gameObject);
		canBeFrozen = false;
		targetObject = GameObject.FindObjectOfType<PlayerControl> ().gameObject;
		fireTarget = targetObject.transform;
	}

	void FixedUpdate(){
		//apply moveVector as a force based on moveSpeed
		rbody.AddForce(maxSpeed * moveVector.normalized);
	}

	override protected void OnInHoverDistance(){
		moveVector = Vector3.zero;
		//fire shot?
	}

	//
	// ATTACKS
	//

	void FireCircleWave(){
		fireDirection = fireTarget.position - transform.position;
		StartCoroutine(CircleWaveTimer());
	}

	IEnumerator CircleWaveTimer(){
		for (int n = 0; n < circleShotCount; n++) {
			for (int s = -1; s <= 1; s += 2){
				//fire circle shot
				circleShotInstance = Instantiate (circleShotPrefab, transform.position, Quaternion.identity);
				circleShotInstance.Initialize (circleSpeed * fireDirection.normalized, Team.enemy);
				circleShotInstance.angVelocity *= s; //fire both clockwise and counterclockwise
			}
			yield return new WaitForSeconds (circleShotInterval);
		}
	}


	void FireSpreadWave(){
		StartCoroutine (SpreadWaveTimer ());
	}

	IEnumerator SpreadWaveTimer(){
		//fire straight shot in a spread pattern
		for (int n = 0; n < spreadWaveCount; n++) {
			for (float angle = -spreadRange / 2; angle <= spreadRange / 2; angle += spreadRange / (spreadCount - 1)) {
				fireDirection = fireTarget.position - transform.position;

				spreadShotInstance = Instantiate (spreadShotPrefab, transform.position, Quaternion.identity);
				spreadShotInstance.transform.up = fireDirection; //point forward, initially
				spreadShotInstance.transform.eulerAngles += new Vector3 (0, 0, angle); //rotate according to spread
				spreadShotInstance.Initialize (spreadShotInstance.transform.up * spreadSpeed, Team.enemy); //launch in new direction
			}
			yield return new WaitForSeconds (spreadShotInterval);
		}
	}

	void StartBlocking(){
		//launch blocking shots
		for (float angle = 0; angle < 360; angle += 360f / (blockerCount - 1)) {
			blockerShotInstance = Instantiate (blockerShotPrefab, transform.position, Quaternion.identity);
			Vector3 posVector = new Vector3 (Mathf.Cos (Mathf.Deg2Rad * angle), Mathf.Sin (Mathf.Deg2Rad * angle), 0);
			blockerShotInstance.Initialize (posVector * blockerInitialSpeed, Team.enemy); //initial velocity ignored
		}
	}

	void FireTrap(){
		//launch trap shots
		for (float angle = 0; angle < 360; angle += 360f / (trapCount - 1)) {
			Vector3 posVector = new Vector3 (Mathf.Cos (Mathf.Deg2Rad * angle), Mathf.Sin (Mathf.Deg2Rad * angle), 0);
			posVector = posVector.normalized * trapRadius;
			trapShotInstance = Instantiate (trapShotPrefab, fireTarget.position + posVector, Quaternion.identity);
			trapShotInstance.targetPosition = fireTarget.position;
			trapShotInstance.Initialize (posVector * trapInitialSpeed, Team.enemy);
		}
	}

	override public IEnumerator Die(){
		GameSaver.liveSave.bossKilled[1] = true;
		GameSaver.liveSave.watertutorialpoint = true;

		Instantiate(deathFlashPrefab, transform.position, Quaternion.identity);
		teleportExit.SetActive(true);
		StartCoroutine(base.Die());
		yield return null;
	}

}
