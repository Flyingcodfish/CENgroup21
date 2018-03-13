using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : Actor {

    public inventory inventory;

    //control fields
    private Vector2 input;

	//attack fields
    private float attackTime = 0.3f; // how long it takes to attack
    private float attackTimer; // time remaining till the attack ends
    private bool attacking = false;

	public Hitbox attackHitbox;
	private Vector2 attackHitboxOffset;


	//behavior begins
	public override void ActorStart(){
		//pass
	}
		
	void OnDestroy(){
		//TODO: death animation, game over screen, etc.
		//currently just loads main menu to avoid crashing the game
		if (isDying) UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
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
            animator.SetInteger("Direction", 0);
			attackHitboxOffset.x = 0;
			attackHitboxOffset.y = 0.5f;
        }
        else if (input.y < 0) //down
        {
            animator.SetInteger("Direction", 1);
			attackHitboxOffset.x = 0;
			attackHitboxOffset.y = -0.5f;
        }
        else if (input.x > 0) //right
        {
            animator.SetInteger("Direction", 3);
			attackHitboxOffset.x = 0.5f;
			attackHitboxOffset.y = -0.25f;
        }
        else if (input.x < 0) //left
        {
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
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Item")
        {
            inventory.AddItem(collision.GetComponent<Item>());
        }
    }
}
