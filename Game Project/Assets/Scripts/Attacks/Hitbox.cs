using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Hitbox : MonoBehaviour {

	private Collider2D hitbox;

	public float damage;
	private Actor parentActor;
	private Team parentTeam; //doesn't need a team component, that's a little overkill? Might be better to be consistent, but this'll do for now.

	private bool active;
	public bool isActive{
		get{
			return this.active;
		}
		set{
			this.active = value;
			this.hitbox.enabled = value;
		}
	}


	//declares a delegate method type
	public delegate void DelegateHitGameObject(GameObject hitObj);
	//an instance of the delegate type: can be assigned to by parents
	//a parent can assign a method to this delegate, which will be called whenever the delegate is called
	//allows actors can impose special effects on their targets, or know whether their attacks hit
	public DelegateHitGameObject HitObject; 

	public void Start(){
		this.parentActor = this.GetComponentInParent<Actor>();
		this.parentTeam = this.GetComponentInParent<TeamComponent>().team;
		this.hitbox = this.GetComponent<Collider2D>();
		hitbox.gameObject.layer = LayerMask.NameToLayer("Hitboxes");
		this.isActive = false;
	}

	//attack hitbox hit something
	public void OnTriggerEnter2D(Collider2D other){
		TeamComponent otherTeam = other.gameObject.GetComponent<TeamComponent>();

		if (otherTeam == null || otherTeam.team != this.parentTeam){
			//apply any special on-hit effects
			if (HitObject != null)
				HitObject(other.gameObject);
			//nicely ask the target to take damage
			other.gameObject.SendMessage("TakeDamage", this.damage * parentActor.GetPower(), SendMessageOptions.DontRequireReceiver);
		}
		//else ignore the collision
	}
}
