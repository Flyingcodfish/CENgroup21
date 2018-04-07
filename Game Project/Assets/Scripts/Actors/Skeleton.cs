using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pathfinding;

public class Skeleton : AI_Actor {

	//attacking fields
	float lastAttackTime;
	public float attackCooldown = 2f;
	public Hitbox attackHitbox;

	//I childproofed my own code, because I am a child :^)
	override public void AI_Start () {
		//pass
	}

	//apply forces based on movement trajectory
	void FixedUpdate(){
		//forces
		if (!animator.GetBool("Attacking") && this.IsActive()){
			rbody.AddForce(Vector3.ClampMagnitude(moveVector, 1f) * this.maxSpeed);
		}
	}

	//define behavior on death, adds death animation
	override public IEnumerator Die(){
		//signal that the actor is dying; AI should halt
		this.isDying = true;

		//wait for freeze effect to wear off; collider and renderer should be on until then
		while (frozenStatus > 0)
			yield return null;

		animator.SetTrigger("Die");
		//turn physics off
		this.GetComponent<Collider2D>().enabled = false;

		yield return new WaitForSeconds(5f); //wait for death animation to finish; TODO: THIS IS A BAD SOLUTION

		//wait for important coroutines to finish
		while (isBusy == true)
			yield return null;

		Destroy(this.gameObject);
	}

	//control animation and attacking
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

	//add damage taking animation
	override public void TakeDamage(int damage){
		animator.SetTrigger("TakeDamage");
		base.TakeDamage(damage);
	}
		
}
