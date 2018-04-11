using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : Actor {
    // inventory fields 
    public inventory inventory;
    private inventory chest;
    private inventory shop;

    public int coins;

    //status fields
    public int hasKeys = 0; //number of door keys owned by the player.
	public int hasMoney = 0; //amount of generic currency owned by the player.
	public float currentMana {get; private set;} //used to cast spells
	public float maxMana = 49; //this is a joke
	public float manaRegen = 100f * 60 / 30; //implemented as "percent of max mana restored per minute." This completely fills the bar in 30 seconds.
	private float manaTickTime = 0.1f; // how often in seconds should mana be regenerated? larger values make mana regen more visible, but also choppier

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
	private bool devConsoleEnabled = false;
	public GameObject devConsole;

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
		StartCoroutine (ManaRegen ());
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
			if (Input.GetKeyDown(KeyCode.BackQuote)){
				devConsoleEnabled = !devConsoleEnabled;

				Time.timeScale = devConsoleEnabled ? 0f : 1f;
				devConsole.SetActive(devConsoleEnabled);
				if (devConsoleEnabled) devConsole.GetComponent<UnityEngine.UI.InputField>().ActivateInputField();
				this.enabledAI = !devConsoleEnabled;
			}

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

        // Used for inventory stuff 
        if (Input.GetKeyDown(KeyCode.I)) // opens inventory 
        {
            Debug.Log("I key pressed");
            inventory.Open();
        }
        // use specific items based on which num is used 
        if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("1 is Pressed");
            inventory.UseItem(0);
        }
        if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("2 is Pressed");
            inventory.UseItem(1);
        }
        if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("3 is Pressed");
            inventory.UseItem(2);
        }
        if (Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Alpha4))
        {
            Debug.Log("4 is Pressed");
            inventory.UseItem(3);
        }
        if (Input.GetKeyDown(KeyCode.Keypad5) || Input.GetKeyDown(KeyCode.Alpha5))
        {
            Debug.Log("5 is Pressed");
            inventory.UseItem(4);
        }
        if (Input.GetKeyDown(KeyCode.Keypad6) || Input.GetKeyDown(KeyCode.Alpha6))
        {
            Debug.Log("6 is Pressed");
            inventory.UseItem(5);
        }
        if (Input.GetKeyDown(KeyCode.Keypad7) || Input.GetKeyDown(KeyCode.Alpha7))
        {
            Debug.Log("7 is Pressed");
            inventory.UseItem(6);
        }
        if (Input.GetKeyDown(KeyCode.Keypad8) || Input.GetKeyDown(KeyCode.Alpha8))
        {
            Debug.Log("8 is Pressed");
            inventory.UseItem(7);
        }
        if (Input.GetKeyDown(KeyCode.Keypad9) || Input.GetKeyDown(KeyCode.Alpha9))
        {
            Debug.Log("9 is Pressed");
            inventory.UseItem(8);
        }
        if (Input.GetKeyDown(KeyCode.Keypad0) || Input.GetKeyDown(KeyCode.Alpha0))
        {
            Debug.Log("0 is Pressed");
            inventory.UseItem(9);
        }
        // end hot bar keys 
        // chest and interact key 
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(chest != null)
            {
                chest.Open();
            }
            if(shop != null)
            {
                shop.Open();
            }
        }
        // used for debugging save and load **********************
        // weird problems when i use key for saving, reads in string and then saves null string right after 
        if (Input.GetKeyDown(KeyCode.K))
        {
            inventory.SaveInventory();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            inventory.LoadInventory(string.Empty); // set to empty to load from pref 
        }
        // ******************************************************
    }


    //returns true if the amount was available to spend, else false.
    //accepts either ints or floats, just for simplicity
    public bool SpendMana(int amount){
		return SpendMana ((float)amount);
	}
	public bool SpendMana(float amount){
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
			iceShardInstance.Initialize(facing * 0.15f, this.teamComponent.team, this.power);
		}
	}


	public void CastFire(){
		if (bombTimer <= 0 && SpendMana(bomb_manaCost)){
			bombTimer = bombTime;
			FireBomb bomb = Instantiate<FireBomb>(bomb_object, transform.position + (Vector3)facing*spellDistance, Quaternion.identity);
			bomb.Initialize(facing * 0.12f, this.teamComponent.team, this.power);
		}
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Item")
        {
            inventory.AddItem(collision.GetComponent<Item>());
            Destroy(collision.gameObject);
        }
        if(collision.tag == "Chest")
        {
            chest = collision.GetComponent<ChestScript>().chestInventory; // gets inventory of chest 
        }
        if(collision.tag == "Shop")
        {
            shop = collision.GetComponent<ShopScript>().shopInventory;// gets inventory of any shops 
        }
        if(collision.tag == "Coins") // gets the type of coin and then adds to total 
        {
           CoinType tmp = collision.GetComponent<CoinScript>().type;
            collision.GetComponent<CoinScript>().AddCoins(tmp);
            Destroy(collision.gameObject);
           
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Chest")
        {
            if (chest.IsOpen) // if open 
            {
                chest.Open(); // closes 
            }
            chest = null; // resets to null to make unable to access unless by chest 
        }
        if (collision.tag == "Shop")
        {
            if (shop.IsOpen) // if open 
            {
                shop.Open(); // closes 
            }
            shop = null; // resets to null to make unable to access unless by chest 
        }
    }

	IEnumerator ManaRegen(){
		while (true) {
			if (currentMana < maxMana) {
				currentMana += (manaRegen / 100f) * maxMana / 60 * manaTickTime; //weird math; makes mana regen operate as "percent of max mana restored per minute"
			}
			yield return new WaitForSeconds (manaTickTime);
		}
	}

}
