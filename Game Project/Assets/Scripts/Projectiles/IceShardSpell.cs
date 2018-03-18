using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

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

	public GameObject iceBlockPrefab;
	public GameObject death_object;

	private bool isBusy = false;


	public void OnTriggerEnter2D(Collider2D other){
		//we hit something. if it is a wall, or on another team, "hit" it
		Actor hitActor = other.gameObject.GetComponent<Actor>();
		if (hitActor == null){
			//we hit a wall; shatter
			StartCoroutine(Die());
		}
		else if (hitActor.team != this.team){
			//nicely ask the target to take damage
			hitActor.SendMessage("TakeDamage", this.damage);
			//freeze the actor for a bit

			StartCoroutine(FreezeActor(hitActor));
//			StartCoroutine(hitActor.Freeze());


			StartCoroutine(Die());
		}
		//else ignore the collision
	}


	IEnumerator FreezeActor(Actor actor){
		isBusy = true;

		bool firstToFreeze = (actor.frozenStatus == 0);
		bool createdSortingGroup = false;
		GameObject iceBlockInstance = null;
		SortingGroup sGroup = null;

		//actor not already frozen; freeze it
		if (firstToFreeze){
			//create iceblock object
			iceBlockInstance = Instantiate(iceBlockPrefab, actor.transform);
			iceBlockInstance.GetComponent<SpriteMask>().sprite = actor.spriteRenderer.sprite;
			sGroup = actor.gameObject.GetComponent<SortingGroup>();
			createdSortingGroup = (sGroup == null);
			if (createdSortingGroup)
				sGroup = actor.gameObject.AddComponent<SortingGroup>();

			//freeze actor
			actor.animator.enabled = false;
			actor.enabledAI = false;
		}

		actor.frozenStatus += 1;
		yield return new WaitForSeconds(freezeTime);
		actor.frozenStatus -= 1;

		//we froze in the first place, so we're responsible for cleaning up
		if (firstToFreeze){
			//wait for other freeze effects to end
			while (actor.frozenStatus > 0)
				yield return null;

			if (actor != null){
				//unfreeze actor
				actor.animator.enabled = true;
				actor.enabledAI = true;

				//destroy iceblock
				if (createdSortingGroup) //don't want to delete this if the actor already had one
					Destroy(sGroup);
				Destroy(iceBlockInstance);
			}
		}
		isBusy = false;
	}
		

	IEnumerator Die(){
		if (isBusy){
			this.GetComponent<SpriteRenderer>().enabled = false;
			this.GetComponent<Collider2D>().enabled = false;
			ParticleSystem.EmissionModule pSysEmitter = this.GetComponent<ParticleSystem>().emission;
			pSysEmitter.enabled = false;
		
		}
		//wait for all coroutines to finish
		while (isBusy)
			yield return null;

		//destroy this bullet
		//instantiate a fade-out effect/object
		if (death_object != null)
			Instantiate(death_object, transform.position, Quaternion.identity);
		Destroy(this.gameObject);
	}


	public void Initialize(Vector2 velocity, Team team){
		this.velocity = velocity;
		this.team = team;
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