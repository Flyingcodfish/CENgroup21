using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//projectile should have a trigger collider2D so it can collide with, but not influence, things
[RequireComponent(typeof(Collider2D))]
//projectile should have a kinematic rigidbody, so it can collide with static terrain
[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour {
	private Team team;
	private Vector2 velocity;
	public int damage = 10;
	public float lifespan = 5f;

	public GameObject death_object;

	public void OnTriggerEnter2D(Collider2D other){
		//we hit something. if it is a wall, or on another team, "hit" it and destroy the bullet.
		Actor hitActor = other.gameObject.GetComponent<Actor>();
		if (hitActor == null){
			this.Die();
		}
		else if (hitActor.team != this.team){
			//nicely ask the target to take damage
			hitActor.SendMessage("TakeDamage", this.damage);
			this.Die();
		}
		//else ignore the collision
	}

	private void Die(){
		//probably hit a wall or target; destroy this bullet
		//instantiate a fade-out effect/object
		Instantiate(death_object, transform.position, Quaternion.identity);
		Destroy(this.gameObject);
	}

	public void Initialize(Vector2 velocity, Team team, float dmgMod){
		this.velocity = velocity;
		this.team = team;
		this.damage = Mathf.RoundToInt(damage * dmgMod);

		StartCoroutine(LifespanCountdown()); //begin the inevitable spiral towards death
	}
		
	public void FixedUpdate(){
		transform.position = transform.position + (Vector3)velocity;
	}

	IEnumerator LifespanCountdown(){
		yield return new WaitForSeconds(lifespan);
		this.Die();
	}
}