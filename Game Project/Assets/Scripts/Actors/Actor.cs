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

<<<<<<< HEAD
    //strength fields
    float strength=1f;
    protected bool strengthUp = false;


=======
    //strength & power fields
	public float power = 1f;
	float strength=1f;
    protected bool strengthUp = false;

>>>>>>> master
	//health and damage fields
	public bool isInvincible = false;
	public int maxHealth;
	public int currentHealth{get; protected set;} //outsiders should not set directly, but use takeDamage()
	public Team team = Team.neutral; //default team

	protected Color hurtColor = new Color32(255, 143, 143, 255);
    protected Color healColor = Color.green; // *could make cool Color32* 
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
            if (strengthUp)
            {
                amount = (int) (amount * strength);
                currentHealth -= amount;
            }
            else
            {
                currentHealth -= amount;
            }
            if (amount < 0)
            {
                StartCoroutine(AnimateHealth());
            }
            else
            {
                StartCoroutine(AnimateDamage());
            }
			if (currentHealth <= 0 ) StartCoroutine(Die());
            if (currentHealth > maxHealth) currentHealth = maxHealth; // needed for health potions to not overheal, also was made invincible if overhealed 
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
    IEnumerator AnimateHealth()
    {
        Color baseColor = this.sprite.color;
        int ticker = 0;

        for (float t = 0; t < iFrameTime; t += flashPeriod / 2)
        {
            //toggle between normal color and hurtColor
            this.sprite.color = (ticker++ % 2 == 0) ? healColor : baseColor;
            yield return new WaitForSeconds(flashPeriod / 2);
        }

        sprite.color = baseColor;
    }

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
    public void ModifyEffect(Actor actor, float Modifier, float Duration, ItemType item) // could make it possible to have effects with non item identifier 
    {
        switch (item)
        {
            case ItemType.SWIFT:
                StartCoroutine(SpeedUp(actor, Modifier, Duration));
                break;
            case ItemType.STRENGTH:
                StartCoroutine(StrengthUp(actor, Modifier, Duration));
                break;
            case ItemType.POWER:
                StartCoroutine(PowerUp(actor, Modifier, Duration));
                break;
        }
        
    }
    IEnumerator SpeedUp(Actor actor, float speedModifier, float Duration) // speeds up actor 
    {
        actor.isBusy = true;
        float baseSpeed = this.maxSpeed;
        actor.maxSpeed = baseSpeed * speedModifier;
        yield return new WaitForSeconds(Duration);
        actor.maxSpeed = baseSpeed;
        actor.isBusy = false;
    }
    IEnumerator StrengthUp(Actor actor, float strengthModifier, float Duration) // makes actor take less damage
    {
        actor.isBusy = true;
        actor.strengthUp = true;
        float baseStrength = this.strength;
        actor.strength = strengthModifier;
        yield return new WaitForSeconds(Duration);
        actor.strength = baseStrength;
        actor.strengthUp = false;
        actor.isBusy = false;
    }
    IEnumerator PowerUp(Actor actor, float powerModifier, float Duration) // makes actor do more damage
    {
<<<<<<< HEAD
        return null;
=======
		actor.isBusy = true;
		float basePower = this.power;
		actor.power = powerModifier;
		yield return new WaitForSeconds(Duration);
		actor.power = basePower;
		actor.isBusy = false;
>>>>>>> master
    }

}



//makes AI behaviors more flexible; allows projectiles to ignore friendlies
public enum Team {
	neutral, player, enemy, special
};