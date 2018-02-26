using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor {

	[SerializeField] //makes private "inVector" visible in inspector for testing
	private Vector2 inVector;

    public override void ActorStart (){
		//pass
    }
		
	//occurs at a framerate-independant rate; used for physics 
	void FixedUpdate () {
		inVector.x = Input.GetAxisRaw("Horizontal");
		inVector.y = Input.GetAxisRaw("Vertical");

		inVector = Vector2.ClampMagnitude(inVector, 1f); //prevents diagonal movement from being faster than orthogonal movement
		rbody.AddForce(inVector * maxSpeed);
	}
    
	//occurs every frame
	//src: http://michaelcummings.net/mathoms/creating-2d-animated-sprites-using-unity-4.3
    void Update()
    {
        if (inVector.y > 0) 	//up
        {
            animator.SetInteger("Direction", 1);
        }
        else if (inVector.y < 0) //down
        {
            animator.SetInteger("Direction", 2);
        }
        else if (inVector.x < 0) //left
        {
            animator.SetInteger("Direction", 3);
			sprite.flipX = true;
        }
        else if (inVector.x > 0) //right
        {
            animator.SetInteger("Direction", 3);
			sprite.flipX = false;
        }
		else
			animator.SetInteger("Direction", 0); //idle
    }
}
