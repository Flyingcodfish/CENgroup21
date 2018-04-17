using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fairy_Queen : Actor {

	//attack fields
	public Projectile circleShotPrefab;
	private Projectile circleShotInstance;
	public float circleSpeed = 0.5f;
	public float circleShotInterval = 0.1f;
	public int circleShotCount = 4;

	public dagger_shot daggerShotPrefab;
	private dagger_shot daggerShotInstance;
	public float daggerSpeed = 0.2f;
	public float daggerSpreadRange = 60f;
	public int daggerSpreadCount = 4;

	public fan_shot fanShotPrefab;
	private fan_shot fanShotInstance;
	public float fanSpeed = 0.2f;
	public float fanShotInterval = 0.1f;
	public int fanShotCount = 3;

	private Transform fireTarget;
	private Vector2 fireDirection;

	private float moveCooldown =  3.2f;
	private float attackCooldown = 2f;

	private Vector2 moveVector;
	private Vector3 moveTarget;
	private float maxMoveDist = 4f;
	private float moveDeadZone = 0.2f;

	public GameObject deathFlashPrefab;

	override public void ActorStart(){
		fireTarget = GameObject.FindObjectOfType<PlayerControl>().transform;
		canBeFrozen = false;
		StartCoroutine(MoveTimer());
		StartCoroutine(AttackTimer());
	}


//	void Update(){
//		//test fire
//		if (Input.GetKeyDown(KeyCode.F)){
//			FireCircleWave();
//		}
//
//		if (Input.GetKeyDown(KeyCode.G)){
//			FireDaggerWave();
//		}
//
//		if (Input.GetKeyDown(KeyCode.H)){
//			FireFanWave();
//		}
//	}

	//AI BEHAVIOR

	void FixedUpdate(){
		moveVector = moveTarget - transform.position;
		if (moveVector.magnitude > moveDeadZone)
			rbody.AddForce(moveVector.normalized * maxSpeed);
	}

	IEnumerator MoveTimer(){
		while (true){
			yield return new WaitForSeconds(moveCooldown);

			//randomly select a move target point
			float x = Random.Range(-1f, 1f);
			float y = Random.Range(-1f, 1f);
			float d = Random.Range(0f, maxMoveDist);
			Vector3 stepVector = new Vector3(x, y, 0f);

			moveTarget = transform.position + stepVector.normalized * d;
		}
	}

	IEnumerator AttackTimer(){
		while(true){
			yield return new WaitForSeconds(attackCooldown);

			fireDirection = fireTarget.position - transform.position;
			int chosenAttack = Random.Range(0, 4);

			if (chosenAttack == 0){
				FireCircleWave();
			}
			else if (chosenAttack == 1) {
				FireDaggerWave();
			}
			else if (chosenAttack == 2) {
				FireFanWave();
			}
			else{
				//give the player a break
			}
		}
	}



	//ATTACK FUNCTIONS

	void FireCircleWave(){
		StartCoroutine(CircleWaveTimer());
	}

	IEnumerator CircleWaveTimer(){
		for (int n = 0; n < circleShotCount; n++){
			//fire circle shot
			circleShotInstance = Instantiate(circleShotPrefab, transform.position, Quaternion.identity);
			circleShotInstance.Initialize(circleSpeed * fireDirection.normalized, Team.enemy);
			yield return new WaitForSeconds(circleShotInterval);
		}
	}

	void FireDaggerWave(){
		//fire dagger shot in a spread pattern
		for (float angle = -daggerSpreadRange/2; angle <= daggerSpreadRange/2; angle += daggerSpreadRange/(daggerSpreadCount-1)){
			daggerShotInstance = Instantiate(daggerShotPrefab, transform.position, Quaternion.identity);
			daggerShotInstance.transform.up = fireDirection; //point forward, initially
			daggerShotInstance.transform.eulerAngles += new Vector3(0, 0, angle); //rotate according to spread
			daggerShotInstance.target = fireTarget;
			daggerShotInstance.Initialize(daggerShotInstance.transform.up * daggerSpeed, Team.enemy); //launch in new direction
		}
	}

	void FireFanWave(){
		StartCoroutine(FanWaveTimer());
	}

	IEnumerator FanWaveTimer(){
		for (int n = 0; n < fanShotCount; n++){
			//fire fan shot
			fanShotInstance = Instantiate(fanShotPrefab, transform.position, Quaternion.identity);
			fanShotInstance.Initialize(fanSpeed * fireDirection.normalized, Team.enemy);
			yield return new WaitForSeconds(fanShotInterval);
		}
	}


	override public IEnumerator Die(){
		GameSaver.liveSave.bossKilled[1] = true;
		GameSaver.liveSave.watertutorialpoint = true;

		Instantiate(deathFlashPrefab, transform.position, Quaternion.identity);
		StartCoroutine(base.Die());
		return null;
	}

}

