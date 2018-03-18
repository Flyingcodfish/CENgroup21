using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//base class for anything that breathes (and then some)
//set some requirements: all actors should have a body/collider in the world, and should be animated sprites
[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
[RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
public abstract class Actor : MonoBehaviour {

	//bookkeeping fields
	protected bool isBusy = false; //death only destroys object once this is false; used to wait on coroutines that must finish
	public bool enabledAI = true; //used to pause coroutines/movement (used by freezing effects)
	protected bool isDying = false; //similar to the above but "stronger," triggered by death
	public bool IsActive(){return enabledAI && !isDying;} //method for easily checking the last two fields


    //strength & power fields
	public float power = 1f;
	float strength=1f;
    protected bool strengthUp = false;

	//health and damage fields
	public bool isInvincible = false;
	public int maxHealth;
	public int currentHealth{get; protected set;} //outsiders should not set directly, but use takeDamage()
	public Team team = Team.neutral; //default team

	protected Color hurtColor = new Color32(255, 143, 143, 255);
    protected Color healColor = Color.green; // *could make cool Color32* 
	protected float flashPeriod = 0.12f; //period (in seconds) of flashing after getting hurt
	protected float iFrameTime = 0.3f; //length of invincibility after getting hurt

	//status effect fields
	public int frozenStatus = 0; //integer: +1 when frozen, -1 when freezes end. Allows multiple sources to overlap freeze duration, but not stack effects.

	//movement fields
	public float drag = 100f;
	public float maxSpeed = 300f;

	//references to required components
	public Rigidbody2D rbody {get; private set;}
	public Animator animator {get; private set;}
	public SpriteRenderer spriteRenderer {get; private set;}


	//Start is used to initialize important Actor component references
	//inheritors should use ActorStart()
	public void Start(){
		//the following components always exist
		currentHealth = maxHealth;
		rbody = this.GetComponent<Rigidbody2D>();
		animator = this.GetComponent<Animator>();
		spriteRenderer = this.GetComponent<SpriteRenderer>();

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
		//AI should halt
		this.isDying = true;
//		animator.SetTrigger("Die"); //trigger death animation, should be pretty universal

		//wait for freeze effect to wear off
		while (frozenStatus > 0)
			yield return null;

		//turn physics off
		this.GetComponent<Collider2D>().enabled = false;
		this.spriteRenderer.enabled = false;

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
		Color baseColor = this.spriteRenderer.color;
		int ticker = 0;
		this.isInvincible = true;

		for (float t = 0; t < iFrameTime; t += flashPeriod/2){
			//toggle between normal color and hurtColor
			this.spriteRenderer.color = (ticker++ %2 == 0) ? hurtColor : baseColor;
			yield return new WaitForSeconds(flashPeriod/2);
		}

		spriteRenderer.color = baseColor;
		this.isInvincible = false;
	}
    public void ModifyEffect(Actor actor, float Modifier, float Duration, string item) // uses string to allow for non items to call modify effects 
    {
        switch (item)
        {
            case "SWIFT":
                StartCoroutine(SpeedUp(actor, Modifier, Duration));
                break;
            case "STRENGTH":
                StartCoroutine(StrengthUp(actor, Modifier, Duration));
                break;
            case "POWER":
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
		actor.isBusy = true;
		float basePower = this.power;
		actor.power = powerModifier;
		yield return new WaitForSeconds(Duration);
		actor.power = basePower;
		actor.isBusy = false;
    }

}



//makes AI behaviors more flexible; allows projectiles to ignore friendlies
public enum Team {
	neutral, player, enemy, special
};