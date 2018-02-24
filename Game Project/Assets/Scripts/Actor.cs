using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//base class for anything that breathes (and then some)
//set some requirements: all actors should have a body/collider in the world, and should be animated sprites
[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
[RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
public abstract class Actor : MonoBehaviour {

	public int maxHealth;
	public int currentHealth{get; protected set;} //outsiders should not set directly, but use takeDamage()
	public Team team = Team.neutral; //default team

	public float drag = 100f;
	public float maxSpeed = 300f;

	public Rigidbody2D rbody {get; private set;}
	public Animator animator {get; private set;}
	public SpriteRenderer sprite {get; private set;}

	//Start is used to initialize important Actor component references
	//inheritors should use ActorStart()
	public void Start(){
		//the following components always exist
		currentHealth = maxHealth;
		rbody = this.GetComponent<Rigidbody2D>();
		animator = this.GetComponent<Animator>();
		sprite = this.GetComponent<SpriteRenderer>();

		rbody.drag = this.drag;
		rbody.gravityScale = 0;
		rbody.constraints = RigidbodyConstraints2D.FreezeRotation;

		this.ActorStart();
	}

	public virtual void takeDamage(int amount){
		currentHealth -= amount;
		if (currentHealth <= 0 ) die();
	}

	public virtual void die(){
		Destroy(this);
	}

	//must be overridden in inherited classes
	public abstract void ActorStart();

}



//makes AI behaviors more flexible/powerful
public enum Team {
	neutral, player, enemy, special
};