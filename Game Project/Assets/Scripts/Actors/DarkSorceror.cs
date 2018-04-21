using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Pathfinding;

public class DarkSorceror : AI_Actor {

	public GameObject teleportExit;
	public GameObject deathFlashPrefab;

	public Transform fireTarget;
	private Vector3 fireDirection;
	Vector3 localScale;

	private bool attackTrigger;
	private float attackCooldown;
	public float[] attackCooldownRange = {1f, 2f};

	private float teleportTime;
	public float[] teleportTimeRange = {5f, 7f};
	public bool teleporting = false;
	Vector3 teleportLocation;

	public Tilemap teleportMap;
	Vector3Int[] teleportLocationList;

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
	bool isBlocking;
	float blockCooldown = 4f;

	public trap_shot trapShotPrefab;
	private trap_shot trapShotInstance;
	public int trapCount = 8;
	public float trapInitialSpeed = 0.15f;
	public float trapRadius = 2f;

	//
	//BEHAVIOR BEGINS
	//

	void Update(){
//		//for test-firing attacks
//		if (Input.GetKeyDown(KeyCode.F)){
//			FireCircleWave();
//		}
//		if (Input.GetKeyDown(KeyCode.G)){
//			FireSpreadWave();
//		}
//		if (Input.GetKeyDown(KeyCode.H)){
//			StartBlocking();
//		}
//		if (Input.GetKeyDown (KeyCode.J)) {
//			FireTrap ();
//		}
		if (moveVector.magnitude <= moveDeadZone) {	//idle
			animator.SetInteger ("Direction", 0);
		} else if (moveVector.x > 0) {				//right
			localScale.x = Mathf.Abs (localScale.x);
			transform.localScale = localScale;
			animator.SetInteger ("Direction", 4);
			localScale.x = Mathf.Abs(localScale.x);
			transform.localScale = localScale;
		} else if (moveVector.x < 0) {				//left
			localScale.x = Mathf.Abs (localScale.x) * -1;
			transform.localScale = localScale;
			animator.SetInteger ("Direction", 3);
		} else if (moveVector.y < 0) {				//up
			animator.SetInteger ("Direction", 2);
		} else if (moveVector.y > 0) {				//down
			animator.SetInteger ("Direction", 1);
		}
	}

	override public void AI_Start(){
		if (GameSaver.liveSave.bossKilled [2] == true)
			Destroy (this.gameObject);
		canBeFrozen = false;
		targetObject = GameObject.FindObjectOfType<PlayerControl> ().gameObject;
		fireTarget = targetObject.transform;
		localScale = transform.localScale;
		lockAI = 0;
		fireFilter = Navigator.GetFilterFromBlockingType (Navigator.BlockingType.flying, false);
		teleportLocationList = Pathfinding.MapConverter.GetAllTileLocations (teleportMap);
		StartCoroutine (AttackTimer ());
		StartCoroutine (TeleportTimer ());
	}

	void FixedUpdate(){
		//apply moveVector as a force based on moveSpeed
		if (teleporting == false)
			rbody.AddForce(maxSpeed * moveVector.normalized);
	}

	override protected void OnInHoverDistance(){
		moveVector = Vector3.zero;
		if (attackTrigger && !teleporting) {
			LaunchAttack ();
		}
	}

	IEnumerator AttackTimer(){
		while (true) {
			attackCooldown = Random.Range (attackCooldownRange [0], attackCooldownRange [1]);
			yield return new WaitForSeconds (attackCooldown);

			attackTrigger = true;
			while (attackTrigger)
				yield return null;
		}
	}

	IEnumerator BlockTimer(){
		yield return new WaitForSeconds (blockCooldown);
		isBlocking = false;
	}

	IEnumerator TeleportTimer(){
		while (true) {
			teleportTime = Random.Range (teleportTimeRange [0], teleportTimeRange [1]);
			yield return new WaitForSeconds (teleportTime);

			Vector3Int coord = teleportLocationList [Random.Range (0, teleportLocationList.Length)];
			teleportLocation = teleportMap.GetCellCenterWorld (coord);

			StartCoroutine (TeleportAndFade ());
			while (teleporting)
				yield return null;
		}
	}

	IEnumerator TeleportAndFade(){
		teleporting = true;
		float duration = 0.15f; //duration of animation in seconds
		float fadeFramerate = 30; //Hz
		float step = 1f / (duration*fadeFramerate);
		Color spriteColor = spriteRenderer.color;

		//fade out
		for (float alpha = 1f; alpha > 0f; alpha -= step){
			spriteColor.a= alpha; //fade out as size increases
			spriteRenderer.color = spriteColor;
			yield return new WaitForSeconds(1/fadeFramerate);
		}

		//teleport
		transform.position = teleportLocation;

		//fade in
		for (float alpha = 0f; alpha < 1f; alpha += step){
			spriteColor.a= alpha; //fade out as size increases
			spriteRenderer.color = spriteColor;
			yield return new WaitForSeconds(1/fadeFramerate);
		}

		teleporting = false;
	}


	//
	// ATTACKS
	//

	void LaunchAttack(){
		attackTrigger = false;
		int choice = Random.Range (0, (isBlocking ? 4 : 5)); //only allow blocking if we're not already blocking
		switch (choice) {
		case 0:
			//nothing, give player a small break
			return;
		case 1:
			FireCircleWave ();
			break;
		case 2:
			FireSpreadWave ();
			break;
		case 3:
			FireTrap ();
			break;
		case 4:
			StartBlocking ();
			break;
		}
	}


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
