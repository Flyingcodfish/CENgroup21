using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pathfinding;

public class Slime : Actor {
	//navigation fields
	bool pathFound;
	private Navigator navigator;
	public Navigator.BlockingType bType = Navigator.BlockingType.walking;
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
	public float hoverDistance = 1.2f;
	public float moveDeadZone = 0.1f;

	//attacking fields
	float lastAttackTime;
	public float attackCooldown = 2f;
	public Hitbox attackHitbox;
	private Vector2 hitboxOffset = new Vector2(0.5f, 0);
	public float slowTime = 1.5f; //how long a target is slowed when hit
	public float slowRatio = 0.7f; //how much of a target's movespeed remains while slowed

	// Use this for initialization
	override public void ActorStart () {
		targetObject = GameObject.FindWithTag("Player");
		navigator = GameObject.FindWithTag("Navigator").GetComponent<Navigator>();

		obstacleFilter = Navigator.GetFilterFromBlockingType(bType, true);
		tileFilter = Navigator.GetFilterFromBlockingType(bType, false);
		castHits = new RaycastHit2D[maxHits];

		attackHitbox.HitActor = this.SlowActor; //assigns the slime's SlowActor method to the hitbox's delegate

		StartCoroutine(AI_Tick());
	}
		
	void FixedUpdate(){
		//forces
		//only move if slime is both walking and also not in the middle of a bounce
		if (animator.GetBool("Grounded") == false){
			rbody.AddForce(Vector3.ClampMagnitude(moveVector, 1f) * this.maxSpeed);
		}
	}

	void Update(){
		//animation
		if (moveVector.magnitude < moveDeadZone){
			moveVector = Vector3.zero;
			animator.SetBool("Walk", false);
		}
		else{
			animator.SetBool("Walk", true);
			sprite.flipX = (moveVector.x > 0); //slime sprite faces left, flip if sprite moving right
			attackHitbox.transform.localPosition = new Vector2(hitboxOffset.x * ((moveVector.x > 0)? 1 : -1), hitboxOffset.y);
 		}

		//attack hitbox activation
		//TODO there's a better way to do this, but it involves setting up more functions and animation events
		//this is simpler but can be optimized if need be
		if (animator.GetBool("AttackActive")){
			if (attackHitbox.isActive == false){
				attackHitbox.isActive = true;
			}
		}
		else{
			attackHitbox.isActive = false;
		}
	}
		
	//function assigned to attack hitbox delegate. Called whenever hitbox hits something.
	public void SlowActor(Actor actor){
		StartCoroutine(SlowEffect(actor));
		Debug.Log(System.String.Format("{0} has been slowed by {1}!", actor.name, this.name));
	}

	//coroutine used to time the effect of a slime's attacks.
	//the slime's prey should be slowed for a bit
	IEnumerator SlowEffect(Actor actor){
		float baseSpeed = actor.maxSpeed;
		actor.maxSpeed = baseSpeed * slowRatio;
		yield return new WaitForSeconds(slowTime);

		actor.maxSpeed = baseSpeed;
	}

	//only need to perform pathfinding every ~0.1 second; less CPU intensive
	IEnumerator AI_Tick(){
		while (true){
			pathFound = false;
			directMove = targetObject.transform.position - transform.position;

			if (directMove.magnitude <= hoverDistance){
				//no need to get closer, stop moving
				//attack player if not on cooldown
				if (Time.time - lastAttackTime >= attackCooldown){
					lastAttackTime = Time.time;
					animator.SetTrigger("Attack");
				}
				pathFound = true;
				moveVector = Vector3.zero; //move slower than normal, for funsies
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
			yield return new WaitForSeconds(0.1f);
		}
	}
}
