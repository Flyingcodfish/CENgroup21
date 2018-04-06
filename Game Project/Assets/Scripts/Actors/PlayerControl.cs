using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : Actor {

    // inventory and items

    public inventory inventory;

    private inventory chest;

    private inventory shop;

    public int coins;

    //control fields
    private Vector2 input;
	private Vector2 facing = Vector2.up;

	//attack fields
    private float attackTime = 0.3f; // how long it takes to attack
    private float attackTimer; // time remaining till the attack ends
    private bool attacking = false;

	public Hitbox attackHitbox;
	private Vector2 attackHitboxOffset;

	private float spellSpawnDistance = 1f;

    //behavior begins
    public override void ActorStart(){
		//pass
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
		rbody.AddForce(input * maxSpeed);
	}

	//occurs every frame
	//src: http://michaelcummings.net/mathoms/creating-2d-animated-sprites-using-unity-4.3
    void Update()
    {
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
		attackHitbox.isActive = attacking;
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
    }

    public IceShardSpell iceShardPrefab;
	//probably a pretty bad method name
	public void CastIce(){
		IceShardSpell iceShardInstance = Instantiate(iceShardPrefab, this.transform.position + (Vector3)facing*spellSpawnDistance, Quaternion.identity);
		iceShardInstance.transform.up = facing;
		iceShardInstance.Initialize(facing * 0.15f, Team.player);
	}


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Item")
        {
            inventory.AddItem(collision.GetComponent<Item>()); // gets item and destroys the old copy in scene
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
}
