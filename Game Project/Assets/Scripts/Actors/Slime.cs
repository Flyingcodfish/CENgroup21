using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pathfinding;

public class Slime : AI_Actor {

	//attacking fields
	float lastAttackTime;
	public float attackCooldown = 2f;
	public Hitbox attackHitbox;
	public float slowTime = 1.5f; //how long a target is slowed when hit
	public float slowRatio = 0.7f; //how much of a target's movespeed remains while slowed

	// Use this for initialization
	override public void AI_Start () {
		attackHitbox.HitObject = this.SlowObject; //assigns the slime's SlowActor method to the hitbox's delegate
	}

	//define behavior for when player is close enough to attack
	override protected void OnInHoverDistance(){
		//no need to get closer, stop moving
		//attack player if not on cooldown
		if (Time.time - lastAttackTime >= attackCooldown){
			lastAttackTime = Time.time;
			animator.SetBool("Attacking", true);
		}
		moveVector = Vector3.zero; //move slower than normal, for funsies
	}

	//apply forces based on movement trajectory
	void FixedUpdate(){
		//forces
		//only move if slime is both walking and also not in the middle of a bounce
		if (!animator.GetBool("Grounded") && !animator.GetBool("Attacking") && this.IsActive()){
			rbody.AddForce(Vector3.ClampMagnitude(moveVector, 1f) * this.maxSpeed);
		}
	}

	//animation & attacking control
	void Update(){
		//animation
		if (moveVector.magnitude < moveDeadZone){
			moveVector = Vector3.zero;
			animator.SetBool("Walk", false);
		}
		else{
			animator.SetBool("Walk", true);
 		}

		//sprite flipping: setting negative X scale flips EVERYTHING (sprite, but also children objects like colliders)
		if (!animator.GetBool("Attacking")){
			transform.localScale = new Vector3(-1*Mathf.Sign(directMove.x), transform.localScale.y, transform.localScale.z);
		}

		//attack hitbox activation
		//TODO there's a better way to do this, but it involves setting up more functions and animation events
		//this is simpler but can be optimized if need be
		if (animator.GetBool("AttackActive") && this.IsActive()){
			if (attackHitbox.isActive == false){
				attackHitbox.isActive = true;
			}
		}
		else{
			attackHitbox.isActive = false;
		}
	}
		
	//function which is assigned to attack hitbox delegate. Called whenever hitbox hits something.
	public void SlowObject(GameObject hitObject){
		Actor hitActor = hitObject.GetComponent<Actor> ();
		if (hitActor != null)
			hitActor.ModifyEffect (Effect.SpeedUp, slowTime, slowRatio);
	}
}
