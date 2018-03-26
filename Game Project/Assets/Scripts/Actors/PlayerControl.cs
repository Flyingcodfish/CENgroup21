using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : Actor {

    public inventory inventory;

	//status fields
	public int hasKeys = 0; //number of door keys owned by the player.
	public int hasMoney = 0; //amount of generic currency owned by the player.
	public int currentMana {get; private set;} //used to cast spells
	public int maxMana = 49; //this is a joke

	//spell fields
	private float spellDistance = 0.15f;

	public FireBomb bomb_object;
	int bomb_manaCost = 14;
	private float bombTime = 3.0f; //cooldown on firing a bomb
	private float bombTimer; // time remaining until bomb explodes

	public IceShardSpell iceShardPrefab;
	int ice_manaCost =  7;
	private float iceTime = 0.2f;
	private float iceTimer;


    //control fields
    private Vector2 input;
	private Vector2 facing = Vector2.up;

	//attack fields
    private float attackTime = 0.3f; // how long it takes to attack
    private float attackTimer; // time remaining till the attack ends
    private bool attacking = false;
	public Hitbox attackHitbox;
	private Vector2 attackHitboxOffset;


	//behavior begins
	public override void ActorStart(){
		Object.DontDestroyOnLoad(this); //player object should be persistent
		currentMana = maxMana;
	}
		
	void OnDestroy(){
		//TODO: death animation, game over screen, etc.
		//currently just loads main menu to avoid crashing the game
		if (enabledAI) UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
	}

	//occurs at a framerate-independant rate; used for physics 
	void FixedUpdate () {
		input.x = Input.GetAxisRaw("Horizontal");
		input.y = Input.GetAxisRaw("Vertical");

		input = Vector2.ClampMagnitude(input, 1f); //prevents diagonal movement from being faster than orthogonal movement
		if (IsActive())rbody.AddForce(input * maxSpeed);
	}

	//occurs every frame
	//src: http://michaelcummings.net/mathoms/creating-2d-animated-sprites-using-unity-4.3
    void Update()
    {
		if (IsActive()){
			animator.SetBool("Walking", true);
	        if (input.y > 0) 	//up
	        {
				facing = Vector2.up;
				animator.SetInteger("Direction", 0);
				attackHitboxOffset.x = 0;
				attackHitboxOffset.y = 0.5f;
	        }
	        else if (input.y < 0) //down
	        {
				facing = Vector2.down;
	            animator.SetInteger("Direction", 1);
				attackHitboxOffset.x = 0;
				attackHitboxOffset.y = -0.5f;
	        }
	        else if (input.x > 0) //right
	        {
				facing = Vector2.right;
	            animator.SetInteger("Direction", 3);
				attackHitboxOffset.x = 0.5f;
				attackHitboxOffset.y = -0.25f;
	        }
	        else if (input.x < 0) //left
	        {
				facing = Vector2.left;
	            animator.SetInteger("Direction", 2);
				attackHitboxOffset.x = -0.5f;
				attackHitboxOffset.y = -0.25f;
	        }
	        else
	        {
				animator.SetBool("Walking", false);
	//            int dir = animator.GetInteger("Direction");
	//            if(dir == 0) //idle down
	//                animator.SetInteger("Direction", 4);
	//            else if (dir == 1) //idle right
	//                animator.SetInteger("Direction", 5);
	//            else if (dir == 2) //idle up
	//                animator.SetInteger("Direction", 6);
	//            else if (dir == 3) // idle left
	//                animator.SetInteger("Direction", 7); 
	        }
	        if(Input.GetButtonDown("Attack") && !attacking)
	        {
				attacking = true;
	            attackTimer = attackTime;
				attackHitbox.transform.localPosition = attackHitboxOffset;
	        }
	        if (attacking)
	        {
	            if(attackTimer > 0)
	            {
	                attackTimer -= Time.deltaTime;
	            }
	            else
	            {
	                attacking = false; 
	            }
	        }

	        animator.SetBool("Attacking", attacking);
		}

		//timers continue regardless of whether the player is active or not
		if (attacking) {
			if(attackTimer > 0) {
				attackTimer -= Time.deltaTime;
			}
			else {
				attacking = false; 
			}
		}

		//attack hitbox should not stay on if frozen; status updated outside IsActive block
		attackHitbox.isActive = attacking && this.IsActive();

		if (bombTimer >= 0) {
			bombTimer -= Time.deltaTime;
		}

		if (iceTimer >= 0){
			iceTimer -= Time.deltaTime;
		}


    }

	//returns true if the amount was available to spend, else false
	public bool SpendMana(int amount){
		if (currentMana >= amount){
			currentMana -= amount;
		}
		else {
			//TODO sound effect or something, maybe even flash the mana bar
			return false;
		}

		if (currentMana > maxMana) currentMana = maxMana; //may have been given a negative amount; mana gain potions
		return true;
	}


	public void CastIce(){
		if (iceTimer <= 0 && SpendMana(ice_manaCost)){
			iceTimer = iceTime;
			IceShardSpell iceShardInstance = Instantiate(iceShardPrefab, transform.position + (Vector3)facing*spellDistance, Quaternion.identity);
			iceShardInstance.transform.up = facing;
			iceShardInstance.Initialize(facing * 0.15f, Team.player, this.power);
		}
	}


	public void CastFire(){
		if (bombTimer <= 0 && SpendMana(bomb_manaCost)){
			bombTimer = bombTime;
			FireBomb bomb = Instantiate<FireBomb>(bomb_object, transform.position + (Vector3)facing*spellDistance, Quaternion.identity);
			bomb.Initialize(facing * 0.12f, this.team, this.power);
		}
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Item")
        {
            inventory.AddItem(collision.GetComponent<Item>());
            Destroy(collision.gameObject);
        }
    }
}
