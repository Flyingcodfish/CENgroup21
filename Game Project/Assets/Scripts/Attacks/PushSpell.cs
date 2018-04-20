using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//projectile should have a trigger collider2D so it can collide with, but not influence, things
[RequireComponent(typeof(Collider2D))]
//projectile should have a kinematic rigidbody, so it can collide with static terrain
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(TeamComponent))]
public class PushSpell : MonoBehaviour {
	Vector2 velocity;
	int damage = 20;
	float lifespan = 0.6f;
	int force = 4000; 
	public GameObject death_object;
	private TeamComponent teamComponent;
	private Vector2 facing; 

	public void OnTriggerEnter2D(Collider2D other){
		//we hit something. if it is a wall, or on another team, "hit" it
		TeamComponent otherTeam = other.gameObject.GetComponent<TeamComponent>();
		if (otherTeam == null || otherTeam.team != this.teamComponent.team){
			//nicely ask the target to freeze and then take damage
			Actor hitActor = other.gameObject.GetComponent<Actor>();

			if (hitActor != null) {
				Rigidbody2D enemyBody = other.gameObject.GetComponent<Rigidbody2D>();
				enemyBody.AddForce (velocity * force,ForceMode2D.Impulse);
				this.Die();
			}

			Projectile enemyProjectile = other.gameObject.GetComponent<Projectile> ();

			if (enemyProjectile != null) {
				otherTeam.team = Team.player;
				enemyProjectile.velocity *= -1;
				enemyProjectile.transform.up = enemyProjectile.velocity;
			}

			other.gameObject.SendMessage("TakeDamage", this.damage, SendMessageOptions.DontRequireReceiver);


		}
		//else ignore the collision
	}

	private void Die(){
		//disable this projectile
		this.GetComponent<SpriteRenderer>().enabled = false;
		this.GetComponent<Collider2D>().enabled = false;
		//engage in self-destructive behavior, like all the cool kids
		//instantiate a fade-out effect/object
		if (death_object != null)
			Instantiate(death_object, transform.position, Quaternion.identity);
		Destroy(this.gameObject);
	}


	public void Initialize(Vector2 velocity, Team team,float dmgMod = 1f){
		this.velocity = velocity;
		this.teamComponent = this.GetComponent<TeamComponent>();
		this.teamComponent.team = team;
		this.damage = (int) (damage * dmgMod);
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
