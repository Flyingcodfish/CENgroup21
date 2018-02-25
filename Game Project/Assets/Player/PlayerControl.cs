using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : Actor {

	private Vector2 input;

    private float attackTime = 0.3f; // how long it takes to attack
    private float attackTimer; // time remaining till the attack ends
    private bool attacking = false;

	public override void ActorStart(){
		//pass
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
        if (input.y > 0) 	//up
        {
            animator.SetInteger("Direction", 2);
        }
        else if (input.y < 0) //down
        {
            animator.SetInteger("Direction", 0);
        }
        else if (input.x > 0) //right
        {
            animator.SetInteger("Direction", 1);
        }
        else if (input.x < 0) //left
        {
            animator.SetInteger("Direction", 3);
        }
        else
        {
            int dir = animator.GetInteger("Direction");
            if(dir == 0) //idle down
                animator.SetInteger("Direction", 4);
            else if (dir == 1) //idle right
                animator.SetInteger("Direction", 5);
            else if (dir == 2) //idle up
                animator.SetInteger("Direction", 6);
            else if (dir == 3) // idle left
                animator.SetInteger("Direction", 7); 
        }
        if(Input.GetKeyDown("j") && !attacking)
        {
            attacking = true;
            attackTimer = attackTime;
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
