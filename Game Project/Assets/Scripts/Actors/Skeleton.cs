using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pathfinding;

public class Skeleton : Actor {

	//targeting fields
	private GameObject targetObject;
	private Vector3 moveVector;
	private Vector2 directMove;
	public float hoverDistance = 2f;
	public float moveDeadZone = 0.1f;

	//navigation fields
	bool pathFound;
	private Navigator navigator;
	public Navigator.BlockingType bType = Navigator.BlockingType.walking;
	private Vector3[] path;

	//obstacle avoidance fields
	public float avoidDistance = 1.5f;
	public Vector3 avoidVector;
	public Vector2 hitDirection;
	int maxHits = 1;
	int numHits;
	private RaycastHit2D[] castHits;
	ContactFilter2D obstacleFilter; //sees terrain AND actors
	ContactFilter2D tileFilter; //only sees terrain
	public Collider2D castCollider;

	//attacking fields
	float lastAttackTime;
	public float attackCooldown = 2f;
	public Hitbox attackHitbox;

	// Use this for initialization
	override public void ActorStart () {
		targetObject = GameObject.FindWithTag("Player");
		navigator = GameObject.FindWithTag("Navigator").GetComponent<Navigator>();

		obstacleFilter = Navigator.GetFilterFromBlockingType(bType, true);
		tileFilter = Navigator.GetFilterFromBlockingType(bType, false);
		castHits = new RaycastHit2D[maxHits];

		StartCoroutine(AI_Tick());
	}

	void FixedUpdate(){
		//forces
		if (!animator.GetBool("Attacking") && this.IsActive()){
			rbody.AddForce(Vector3.ClampMagnitude(moveVector, 1f) * this.maxSpeed);
		}
	}

	override public IEnumerator Die(){
		//signal that the actor is dying; AI should halt
		this.isDying = true;

		//wait for freeze effect to wear off; collider and renderer should be on until then
		while (frozenStatus > 0)
			yield return null;

		animator.SetTrigger("Die");
		//turn physics off
		this.GetComponent<Collider2D>().enabled = false;

		yield return new WaitForSeconds(5f); //wait for death animation to finish; TODO: BAD SOLUTION

		//wait for important coroutines to finish
		while (isBusy == true)
			yield return null;

		Destroy(this.gameObject);
	}

	void Update(){
		//animation
		if (moveVector.magnitude < moveDeadZone){
			moveVector = Vector3.zero;
			animator.SetBool("Walking", false);
		}
		else{
			animator.SetBool("Walking", true);
		}

		//sprite flipping; done this way rather than flipping sprite renderer so hitboxes and children are flipped
		if (!animator.GetBool("Attacking")){
			transform.localScale = new Vector3(Mathf.Sign(directMove.x), transform.localScale.y, transform.localScale.z);
		}

		//attack hitbox activation
		//TODO there's a better way to do this, but it involves setting up more functions and animation events
		//this is simpler and can be optimized if need be
		if (animator.GetBool("AttackActive") && this.IsActive()){
			if (attackHitbox.isActive == false){
				attackHitbox.isActive = true;
			}
		}
		else{
			attackHitbox.isActive = false;
		}
	}

	override public void TakeDamage(int damage){
		animator.SetTrigger("TakeDamage");
		base.TakeDamage(damage);
	}

	//only need to perform pathfinding every ~0.1 second; less CPU intensive
	IEnumerator AI_Tick(){
		while (true){
			if (this.IsActive()){
				pathFound = false;
				directMove = targetObject.transform.position - transform.position;

				if (directMove.magnitude <= hoverDistance){
					//no need to get closer, stop moving
					//attack player if not on cooldown
					if (Time.time - lastAttackTime >= attackCooldown){
						lastAttackTime = Time.time;
						animator.SetBool("Attacking", true);
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
				if (numHits > 0 && !castHits[0].collider.gameObject.Equals(targetObject)){
					//add a force that is perpendicular to the path we take to hit an obstacle.
					//This moves simultaneously away from the obstacle, and towards our goal.
					hitDirection = castHits[0].point - ((Vector2)transform.position + castCollider.offset);
					float phi = Vector2.SignedAngle(moveVector, hitDirection);
					float sign = Mathf.Sign(phi); //determines which of two perpendicular paths to take
					avoidVector = new Vector2(sign*hitDirection.y, -1*sign*hitDirection.x); //calculate normal to hitVector
					moveVector = avoidVector.normalized + moveVector.normalized;
				}
			}
			yield return new WaitForSeconds(0.1f);
		}
	}
}
