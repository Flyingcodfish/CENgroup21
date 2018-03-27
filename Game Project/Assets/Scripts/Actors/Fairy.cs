using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pathfinding;

public class Fairy : Actor {
	//navigation fields
	bool pathFound;
	private Navigator navigator;
	public Navigator.BlockingType bType = Navigator.BlockingType.flying;
	private Vector3[] path;

	//obstacle avoidance fields
	public float avoidDistance = 1.5f;
	Vector3 avoidVector;
	Vector2 hitDirection;
	int maxHits = 1;
	int numHits;
	private RaycastHit2D[] castHits;
	ContactFilter2D obstacleFilter; //sees terrain AND actors
	ContactFilter2D tileFilter; //only sees terrain
	public Collider2D castCollider;

	//targeting fields
	private GameObject targetObject;
	private Vector3 moveVector;
	private Vector2 directMove;
	public float moveDeadZone = 0.1f;
	public float sightRange = 15f;			//Enemies will remain passive until player within sight range 

	//attack behavior fields
	public float hoverDistance = 4f;
	public float attackRange = 10f;
	public Projectile bullet_object;
	public float bulletSpeed = 0.15f;
	public GameObject flash_object;

	// Use this for initialization
	override public void ActorStart () {
		targetObject = GameObject.FindWithTag("Player");
		navigator = GameObject.FindObjectOfType<Navigator>();

		obstacleFilter = Navigator.GetFilterFromBlockingType(bType, true);
		tileFilter = Navigator.GetFilterFromBlockingType(bType, false);
		castHits = new RaycastHit2D[maxHits];

		StartCoroutine(AI_Tick());
	}

	// Update is called once per frame
	void Update () {
			
		if (rbody.velocity.y > moveDeadZone){ 	//up
			animator.SetInteger("Direction", 0);
		}
		else if (rbody.velocity.y < -moveDeadZone){ //down
			animator.SetInteger("Direction", 1);
		}
		else if (rbody.velocity.x < moveDeadZone){ //left
			animator.SetInteger("Direction", 2);
		}
		else if (rbody.velocity.x > -moveDeadZone){ //right
			animator.SetInteger("Direction", 3);
		}
		else {
			//leave facing as is
		}
	}

	void FixedUpdate(){
		//add forces to entity's rigidbody
		if (this.IsActive())
			rbody.AddForce(Vector3.ClampMagnitude(moveVector, 1f) * this.maxSpeed);
	}

	//fires bullet at target
	IEnumerator FireShot(){
		GameObject flash = Instantiate(flash_object, this.transform);
		float maxSize = 0.6f;
		float minSize = 0f;
		float duration = 0.1f; //duration of animation in seconds
		float fadeFramerate = 30; //Hz
		//in units:  (scale-scale) / (seconds * frames/second) => scale/frame
		float step = (maxSize-minSize) / (duration*fadeFramerate);

		for (float flashSize = maxSize; flashSize > minSize; flashSize -= step){
			flash.transform.localScale = Vector3.one * flashSize;
			yield return new WaitForSeconds(1/fadeFramerate);
		}
		Destroy(flash);
		Projectile bullet = Instantiate<Projectile>(bullet_object, transform.position, Quaternion.identity); //no rotation for now
		Vector2 shotDirection = targetObject.transform.position - transform.position;
		bullet.Initialize(shotDirection.normalized * bulletSpeed, this.teamComponent.team, this.power);
	}

	//only need to perform pathfinding every ~0.1 second; less CPU intensive
	IEnumerator AI_Tick(){
		int attackTicker = 0;
		while (true){
			if (this.IsActive()){
				pathFound = false;
				directMove = targetObject.transform.position - transform.position;

				//if within "hover range" of target
				if (directMove.magnitude <= hoverDistance){
					//no need to get closer, circle around target
					avoidVector = new Vector2(directMove.y, -1*directMove.x); //calculate normal to hitVector
					moveVector = avoidVector.normalized + moveVector.normalized;

					pathFound = true;
					moveVector = moveVector.normalized * 0.5f; //move slower than normal, for funsies
				}

				//check if we can (and have to) run straight towards player
				if (pathFound == false){
					if (0 == castCollider.Cast(directMove, tileFilter, castHits, directMove.magnitude)){
						moveVector = directMove; 
						pathFound = true;
					}
				}

				//pathfinding; only if we must
				if (pathFound == false){
					path = navigator.GetWorldPath(bType, transform.position, targetObject.transform.position);
					if (path.Length > 0){
						moveVector = (path[0]-transform.position).normalized;
					}
					else moveVector = Vector3.zero;
					pathFound = true;
				}

				//avoid local obstacles
				numHits = castCollider.Cast((Vector2)moveVector, obstacleFilter, castHits, avoidDistance);

				//if we would run into something if we kept moving forward
				if (numHits > 0){
					//add a force that is perpendicular to the path we take to hit an obstacle.
					//This moves simultaneously away from the obstacle, and towards our goal.
					hitDirection = castHits[0].point - ((Vector2)transform.position + castCollider.offset);
					float phi = Vector2.SignedAngle(moveVector, hitDirection);
					float sign = Mathf.Sign(phi); //choose which of two perpendicular paths to take
					avoidVector = new Vector2(sign*hitDirection.y, -1*sign*hitDirection.x); //calculate normal to hitVector
					moveVector = avoidVector.normalized + moveVector.normalized;
				}

				attackTicker++;
				if (attackTicker >= 15 && directMove.magnitude <= attackRange){
					//only attack if we have line of sight
					if (0 == Physics2D.Raycast(transform.position, directMove, tileFilter, castHits, directMove.magnitude)){
						StartCoroutine(FireShot());
						attackTicker = 0;
					}
				}
				if (directMove.magnitude > sightRange) {
					moveVector = Vector3.zero;
				}
			}
			yield return new WaitForSeconds(0.1f);
		}
	}
}
