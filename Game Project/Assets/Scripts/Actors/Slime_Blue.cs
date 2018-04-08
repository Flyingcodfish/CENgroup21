using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pathfinding;

public class Slime_Blue : AI_Actor {

	//attacking fields
	float lastAttackTime;
	public float attackCooldown = 2f;
	public Hitbox attackHitbox;
	public float freezeTime = 0.75f; //how long a target is slowed when hit

	// Use this for initialization
	override public void AI_Start () {
		attackHitbox.HitObject = this.FreezeObject; //assigns the slime's SlowActor method to the hitbox's delegate
	}

	//apply forces in direction of movement trajectory
	void FixedUpdate(){
		//only move if slime is both walking and also not in the middle of a bounce
		if (!animator.GetBool("Grounded") && !animator.GetBool("Attacking") && this.IsActive()){
			rbody.AddForce(Vector3.ClampMagnitude(moveVector, 1f) * this.maxSpeed);
		}
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

	//control animation and attacking
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
		
	//function assigned to attack hitbox delegate. Called whenever hitbox hits something.
	public void FreezeObject(GameObject hitObj){
		Actor actor = hitObj.GetComponent<Actor>();
		WaterTileObject waterObj = hitObj.GetComponent<WaterTileObject>();
		if (actor != null)
			actor.ModifyEffect(Effect.Freeze, freezeTime);
		if (waterObj != null)
			waterObj.Freeze();
	}
}
