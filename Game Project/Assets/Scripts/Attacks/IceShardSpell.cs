using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//projectile should have a trigger collider2D so it can collide with, but not influence, things
[RequireComponent(typeof(Collider2D))]
//projectile should have a kinematic rigidbody, so it can collide with static terrain
[RequireComponent(typeof(Rigidbody2D))]
public class IceShardSpell : MonoBehaviour {

	Team team;
	Vector2 velocity;
	int damage = 10;
	float lifespan = 5f;

	float freezeTime = 3f;

	public GameObject death_object;

	public void OnTriggerEnter2D(Collider2D other){
		//we hit something. if it is a wall, or on another team, "hit" it
		Actor hitActor = other.gameObject.GetComponent<Actor>();
		if (hitActor == null){
			//we hit a wall; shatter
			StartCoroutine(Die());
		}
		else if (hitActor.team != this.team){
			//nicely ask the target to take damage and get frozen
			hitActor.ModifyEffect(Actor.Effect.Freeze, freezeTime);
			hitActor.TakeDamage(this.damage);

			StartCoroutine(Die());
		}
		//else ignore the collision
	}

	IEnumerator Die(){
		//disable this projectile
		Destroy(transform.GetComponentInChildren<WaterFreezer>().gameObject);
		this.GetComponent<SpriteRenderer>().enabled = false;
		this.GetComponent<Collider2D>().enabled = false;
		ParticleSystem pSys = this.GetComponent<ParticleSystem>();
		ParticleSystem.EmissionModule pSysEmitter = pSys.emission; // I hate that this is necessary
		pSysEmitter.enabled = false;
		
		//wait for all particles to disappear
		ParticleSystem.Particle[] parts = new ParticleSystem.Particle[1]; //only need one slot; we just care about the integer return value. If we use null we get 0.
		while (pSys.GetParticles(parts) > 0) //Also, use "ParticleSystem.Particle" b/c UnityEngine.Particle is deprecated and just serves to pollute the namespace. Blizz pls fix
			yield return null;

		//engage in self-destructive behavior, like all the cool kids
		//instantiate a fade-out effect/object
		if (death_object != null)
			Instantiate(death_object, transform.position, Quaternion.identity);
		Destroy(this.gameObject);
	}


	public void Initialize(Vector2 velocity, Team team, float dmgMod = 1f){
		this.velocity = velocity;
		this.team = team;
		this.damage = (int) (damage * dmgMod);
		StartCoroutine(LifespanCountdown()); //begin the inevitable spiral towards death
	}


	public void FixedUpdate(){
		transform.position = transform.position + (Vector3)velocity;
	}


	IEnumerator LifespanCountdown(){
		yield return new WaitForSeconds(lifespan);
		StartCoroutine(Die());
	}
}