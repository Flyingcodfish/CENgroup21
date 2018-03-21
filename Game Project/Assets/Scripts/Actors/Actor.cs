using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//base class for anything that breathes (and then some)
//set some requirements: all actors should have a body/collider in the world, and should be animated sprites
[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
[RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
public abstract class Actor : MonoBehaviour {

	//bookkeeping fields
	protected bool isBusy = false; //death waits for Actor to finish important coroutines
	protected bool isDying = false; //coroutines that should stop once death starts can use this

	//health and damage fields
	public bool isInvincible = false;
	public int maxHealth;
	public int currentHealth{get; protected set;} //outsiders should not set directly, but use takeDamage()
	public Team team = Team.neutral; //default team

	protected Color hurtColor = new Color32(255, 143, 143, 255);
	protected float flashPeriod = 0.12f; //period (in seconds) of flashing after getting hurt
	protected float iFrameTime = 0.3f; //length of invincibility after getting hurt

	//movement fields
	public float drag = 100f;
	public float maxSpeed = 300f;

	//references to required components
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

	public virtual void TakeDamage(int amount){
		if (this.isInvincible == false){
			currentHealth -= amount;

			StartCoroutine(AnimateDamage());
			if (currentHealth <= 0 ) StartCoroutine(Die());
		}
	}

	virtual public IEnumerator Die(){
		//signal that the actor is dying; AI should halt
		this.isDying = true;
//		animator.SetTrigger("Die"); //should be pretty universal

		//turn physics off
		this.GetComponent<Collider2D>().enabled = false;
		this.sprite.enabled = false;

		//wait for important coroutines to finish
		while (isBusy == true)
			yield return null;

		Destroy(this.gameObject);
	}

	//must be overridden in inherited classes
	//done this way so people don't forget it exists!
	public abstract void ActorStart();

	IEnumerator AnimateDamage(){
		Color baseColor = this.sprite.color;
		int ticker = 0;
		this.isInvincible = true;

		for (float t = 0; t < iFrameTime; t += flashPeriod/2){
			//toggle between normal color and hurtColor
			this.sprite.color = (ticker++ %2 == 0) ? hurtColor : baseColor;
			yield return new WaitForSeconds(flashPeriod/2);
		}

		sprite.color = baseColor;
		this.isInvincible = false;
	}
}



//makes AI behaviors more flexible; allows projectiles to ignore friendlies
public enum Team {
	neutral, player, enemy, special
};