using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor {
    //control fields
    private Vector2 input;
    private Vector2 facing = Vector2.up;

    //attack fields
    // Use this for initialization

    public override void ActorStart()
    {
        //pass
    }
    void FixedUpdate()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        input = Vector2.ClampMagnitude(input, 1f); //prevents diagonal movement from being faster than orthogonal movement
        rbody.AddForce(input * maxSpeed);
    }
    // Update is called once per frame
    void Update () {
        animator.SetBool("Walking", true);
        if (input.y > 0) 	//up
        {
            facing = Vector2.up;
            animator.SetInteger("Direction", 0);
        }
        else if (input.y < 0) //down
        {
            facing = Vector2.down;
            animator.SetInteger("Direction", 1);
        }
        else if (input.x > 0) //right
        {
            facing = Vector2.right;
            animator.SetInteger("Direction", 3);
        }
        else if (input.x < 0) //left
        {
            facing = Vector2.left;
            animator.SetInteger("Direction", 2);
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
    
    }   
}
