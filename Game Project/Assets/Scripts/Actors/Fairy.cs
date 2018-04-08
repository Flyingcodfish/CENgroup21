using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pathfinding;

public class Fairy : AI_Actor {

	//attack behavior fields
	public float attackRange = 10f;
	public Projectile bullet_object;
	public float bulletSpeed = 0.15f;
	public GameObject flash_object;
	public float attackCooldown = 1.5f; //in seconds
	private float attackTimer; //used to track attack cooldowns

	//hovering fields; preallocating for performance (or is this made pointless  by the use of the 'new' keyword in the relevant code???)
	Vector3 hoverVector;

	// Use this for initialization
	override public void AI_Start () {
		//pass
	}

	// Update is called once per frame
	void Update () {
		if (attackTimer > 0) attackTimer -= Time.deltaTime;

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

	override protected void OnInHoverDistance(){
		//no need to get closer, circle around target
		hoverVector = new Vector2(directMove.y, -1*directMove.x); //calculate normal to hitVector
		moveVector = hoverVector.normalized + moveVector.normalized;
		moveVector = moveVector.normalized * 0.5f; //move slower than normal, for funsies
	}

	override protected void OnTickEnd(){
		if (attackTimer <= 0 && directMove.magnitude <= attackRange) {
			//only attack if we have line of sight
			if (0 == Physics2D.Raycast(transform.position, directMove, tileFilter, castHits, directMove.magnitude)){
				attackTimer = attackCooldown;
				StartCoroutine(FireShot());
			}
		}
	}

}
