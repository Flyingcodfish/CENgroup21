using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Hitbox : MonoBehaviour {

	private Collider2D hitbox;

	public float damage;
	private Team team;

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

	public Vector2 GetOffset(){
		if (hitbox == null)//start may not have happened yet
			return this.GetComponent<Collider2D>().offset;
		return this.hitbox.offset;
	}
		
	public bool SetOffset(Vector2 offset){
		if (this.hitbox == null) return false;
		this.hitbox.offset = offset;
		return true;
	}

	//declares a delegate method type
	public delegate void DelegateHitActor(Actor actor);
	//an instance of the delegate type: can be assigned to by parents
	//a parent can assign a method to this delegate, which will be called whenever the delegate is called
	//allows actors can impose special effects on their targets, or know whether their attacks hit
	public DelegateHitActor HitActor; 

	public void Start(){
		this.team = this.GetComponentInParent<Actor>().team;
		this.hitbox = this.GetComponent<Collider2D>();
		this.isActive = false;
	}

	//attack hitbox hit something
	public void OnTriggerEnter2D(Collider2D other){
		Actor hitActor = other.gameObject.GetComponent<Actor>();
		if (hitActor != null && hitActor.team != this.team){
			//nicely ask the target to take damage
			if (HitActor != null)
				HitActor(hitActor);
			hitActor.SendMessage("TakeDamage", this.damage);
		}
		//else ignore the collision
	}
}
