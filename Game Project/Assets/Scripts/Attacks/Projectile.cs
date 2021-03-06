﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//projectile should have a trigger collider2D so it can collide with, but not influence, things
[RequireComponent(typeof(Collider2D))]
//projectile should have a kinematic rigidbody, so it can collide with static terrain
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(TeamComponent))] //projectiles need teams so they can check for friendly fire
public class Projectile : MonoBehaviour {
	public Vector2 velocity;
	public int damage = 10;
	public float lifespan = 5f;
	protected TeamComponent teamComponent;
	protected bool initialized = false;

	public GameObject death_object;

	virtual public void OnTriggerEnter2D(Collider2D other){
		//we hit something. if it is a wall, or on another team, "hit" it and destroy the bullet.
		TeamComponent otherTeam = other.gameObject.GetComponent<TeamComponent>();
		if (otherTeam == null || otherTeam.team != this.teamComponent.team){
			//nicely ask the target to take damage
			other.gameObject.SendMessage("TakeDamage", this.damage, SendMessageOptions.DontRequireReceiver); //damage increased by power in Initialize()
			PushSpell push = other.gameObject.GetComponent<PushSpell>();
			if(push == null)this.Die();
		}
		//else ignore the collision
	}

	protected void Die(){
		//probably hit a wall or target; destroy this bullet
		//instantiate a fade-out effect/object
		if (death_object != null)
			Instantiate(death_object, transform.position, Quaternion.identity);
		Destroy(this.gameObject);
	}

	public void Initialize(Vector2 velocity, Team team, float dmgMod = 1f){
		initialized = true;
		this.velocity = velocity;
		this.teamComponent = this.gameObject.GetComponent<TeamComponent>();
		teamComponent.team = team;
		this.damage = Mathf.RoundToInt(damage * dmgMod);

		OnInit();
		StartCoroutine(LifespanCountdown()); //begin the inevitable spiral towards death
	}

	virtual public void OnInit(){
		//pass
	}

	virtual public void FixedUpdate(){
		transform.position = transform.position + (Vector3)velocity;
	}

	IEnumerator LifespanCountdown(){
		yield return new WaitForSeconds(lifespan);
		this.Die();
	}
}