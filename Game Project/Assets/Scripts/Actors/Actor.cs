using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


//base class for anything that breathes (and then some)
//set some requirements: all actors should have a body/collider in the world, and should be animated sprites. They should all be on a team as well.
[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
[RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
[RequireComponent(typeof(TeamComponent))]
public abstract class Actor : MonoBehaviour {

	//bookkeeping fields
	protected bool isBusy = false; //death only destroys object once this is false; used to wait on coroutines that must finish
	public bool enabledAI = true; //used to pause coroutines/movement (used by freezing effects)
	protected bool isDying = false; //similar to the above but "stronger," triggered by death
	public bool IsActive(){return enabledAI && !isDying;} //method for easily checking the last two fields

	//status effect fields; protected so there's no temptation for other objects to set them directly
	protected float power = 1f;
	public float GetPower(){return power;} //used by hitboxes
    public void AddPower(float value) { power += value; // used to add power for upgrades currently 
        if (power > 2.0f) { power = 2.0f; } //hardcaps at double normal 
        Debug.Log("Power is: " + power);
    } // used to upgrade power of actor 
    protected float strength = 1f;
    public void AddStrength(float value) { strength -= value; // used to add strength for upgrades currently 
        if (strength < 0.5f) { strength = 0.5f; } //hardcaps at double normal 
        Debug.Log("Strength is: " + strength); } // used to upgrade strength of actor
	protected int frozenStatus = 0; //integer: +1 when frozen, -1 when freezes end. Allows multiple sources to overlap freeze duration, but not stack effects.

	//health and damage fields
	public bool isInvincible = false;
	public int maxHealth;
	public int currentHealth{get; protected set;} //outsiders should not set directly, but use takeDamage()

	protected Color hurtColor = new Color32(255, 143, 143, 255);
    protected Color healColor = Color.green; // *could make cool Color32* 
	protected float flashPeriod = 0.12f; //period (in seconds) of flashing after getting hurt
	protected float iFrameTime = 0.3f; //length of invincibility after getting hurt

	//movement fields
	public float drag = 100f;
	public float maxSpeed = 300f;
    public void AddSpeed(float value) // used to add speed for upgrades currently 
    {
        maxSpeed += value;
        if (maxSpeed >= 750f)// hardcaps at double normal speed 
        {
            maxSpeed = 750f;
        }
        Debug.Log("Speed is: " + maxSpeed);
    }

	//references to required components
	public Rigidbody2D rbody {get; private set;}
	public Animator animator {get; private set;}
	public SpriteRenderer spriteRenderer {get; private set;}
	public TeamComponent teamComponent{get; private set;}
	private static GameObject iceBlockPrefab;

	//Start is used to initialize important Actor component references
	//inheritors should use ActorStart()
	public void Start(){
		//the following components always exist
		currentHealth = maxHealth;
		rbody = this.GetComponent<Rigidbody2D>();
		animator = this.GetComponent<Animator>();
		spriteRenderer = this.GetComponent<SpriteRenderer>();
		teamComponent = this.GetComponent<TeamComponent>();
		if (iceBlockPrefab == null)	iceBlockPrefab = Resources.Load<GameObject>("IceBlock");

		rbody.drag = this.drag;
		rbody.gravityScale = 0;
		rbody.constraints = RigidbodyConstraints2D.FreezeRotation;

		this.ActorStart();
	}

	public virtual void TakeDamage(int amount){ 
		if (amount < 0 || this.isInvincible == false){
			currentHealth -= (int)(amount*strength);

			if (amount < 0)
				StartCoroutine(AnimateHealth());
			else
				StartCoroutine(AnimateDamage());

			if (currentHealth > maxHealth) currentHealth = maxHealth;
			if (currentHealth <= 0) StartCoroutine(Die());
		}
	}

	virtual public IEnumerator Die(){
		//AI should halt
		this.isDying = true;
//		animator.SetTrigger("Die"); //trigger death animation, should be pretty universal

		//wait for freeze effect to wear off
		while (frozenStatus > 0)
			yield return null;

		//turn physics off, drop stuff if we (or a child object) have a DeathDropper component
		this.BroadcastMessage("Drop", SendMessageOptions.DontRequireReceiver);
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
    
	/* 
	 * COROUTINES:
	 * status effects, damage animations, death
	 * 
	 */

	IEnumerator AnimateHealth()
    {
        Color baseColor = this.spriteRenderer.color;
        int ticker = 0;

        for (float t = 0; t < iFrameTime; t += flashPeriod / 2)
        {
            //toggle between normal color and hurtColor
            this.spriteRenderer.color = (ticker++ % 2 == 0) ? healColor : baseColor;
            yield return new WaitForSeconds(flashPeriod / 2);
        }

        spriteRenderer.color = baseColor;
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

	/*
	 * STATUS EFFECTS
	 * Call modifyEffect on an actor to make them apply an effect to themselves.
	 * 
	 */

	//enum of supported status effects
	public enum Effect{
		SpeedUp, StrengthUp, PowerUp, Freeze
	}

	//"mother" method that calls everything else
	public void ModifyEffect(Actor.Effect effect, float Duration, float Modifier = 1f)
    {
        switch (effect)
        {
		case Effect.SpeedUp:
            StartCoroutine(SpeedUp(Modifier, Duration));
            break;
		case Effect.StrengthUp:
            StartCoroutine(StrengthUp(Modifier, Duration));
            break;
		case Effect.PowerUp:
            StartCoroutine(PowerUp(Modifier, Duration));
            break;
		case Effect.Freeze:
			StartCoroutine (Freeze(Duration));
			break;
		default:
			Debug.Log("Errror: Invalid status effect applied to actor: " + this.gameObject.name);
			break;
        }
    }
    IEnumerator SpeedUp(float speedModifier, float Duration) // speeds up actor 
    {
        float baseSpeed = this.maxSpeed;
        this.maxSpeed = baseSpeed * speedModifier;
        if (this.maxSpeed > 1250) // hardcaps the speed for modifiers 
        {
            maxSpeed = 1250;
        }
        yield return new WaitForSeconds(Duration);
        if (this != null)this.maxSpeed = baseSpeed;
    }
    IEnumerator StrengthUp(float strengthModifier, float Duration) // makes actor take less damage
    {
        float baseStrength = this.strength;
        this.strength = strengthModifier;
        yield return new WaitForSeconds(Duration);
        if (this!=null)this.strength = baseStrength;
    }
    IEnumerator PowerUp(float powerModifier, float Duration) // makes actor do more damage
    {
		float basePower = this.power;
		this.power = powerModifier;
		yield return new WaitForSeconds(Duration);
		if (this!=null)this.power = basePower;
    }
	IEnumerator Freeze(float duration){
		isBusy = true;

		bool firstToFreeze = (frozenStatus == 0);
		bool createdSortingGroup = false;
		GameObject iceBlockInstance = null;
		SortingGroup sGroup = null;

		//actor not already frozen; freeze it
		if (firstToFreeze){
			//create iceblock object
			iceBlockInstance = Instantiate(iceBlockPrefab, transform);
			iceBlockInstance.GetComponent<SpriteMask>().sprite = spriteRenderer.sprite;
			sGroup = GetComponent<SortingGroup>();
			createdSortingGroup = (sGroup == null);
			if (createdSortingGroup)
				sGroup = gameObject.AddComponent<SortingGroup>();

			//freeze actor
			animator.enabled = false;
			enabledAI = false;
		}

		frozenStatus += 1;
		yield return new WaitForSeconds(duration);
		frozenStatus -= 1;

		//we froze in the first place, so we're responsible for cleaning up
		if (firstToFreeze){
			//wait for other freeze effects to end
			while (frozenStatus > 0)
				yield return null;

			if (this != null){
				//unfreeze actor
				animator.enabled = true;
				enabledAI = true;

				//destroy iceblock
				if (createdSortingGroup) //don't want to delete this if the actor already had one
					Destroy(sGroup);
				Destroy(iceBlockInstance);
			}
		}
		isBusy = false;
	}

}
