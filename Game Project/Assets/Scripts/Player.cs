using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor {

	public float maxSpeed;

	private Rigidbody2D rbody;
	public Vector2 input; //only public so it's visible in the inspector; temporary
    private Animator animator;
	private SpriteRenderer sprite;

    public override void Start (){
		base.Start();
		rbody = this.GetComponent<Rigidbody2D>();
        animator = this.GetComponent<Animator>();
		sprite = this.GetComponent<SpriteRenderer>();
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
            animator.SetInteger("Direction", 1);
        }
        else if (input.y < 0) //down
        {
            animator.SetInteger("Direction", 2);
        }
        else if (input.x < 0) //left
        {
            animator.SetInteger("Direction", 3);
			sprite.flipX = true;
        }
        else if (input.x > 0) //right
        {
            animator.SetInteger("Direction", 3);
			sprite.flipX = false;
        }
		else
			animator.SetInteger("Direction", 0); //idle
    }

	public Player() : base(){
		//pass
	}
}
