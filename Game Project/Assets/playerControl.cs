using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerControl : MonoBehaviour {

	public float maxSpeed;

	private Rigidbody2D rbody;
	private Vector2 input;
    private Animator animator;

    void Start (){
		rbody = gameObject.GetComponent<Rigidbody2D>();
        animator = this.GetComponent<Animator>();
    }
		
	void FixedUpdate () {
		input.x = Input.GetAxisRaw("Horizontal") * maxSpeed;
		input.y = Input.GetAxisRaw("Vertical") * maxSpeed;

		rbody.AddForce(input);
	}
    // src: http://michaelcummings.net/mathoms/creating-2d-animated-sprites-using-unity-4.3
    void Update()
    {
        var vertical = Input.GetAxis("Vertical");
        var horizontal = Input.GetAxis("Horizontal");

        if (vertical > 0)
        {
            animator.SetInteger("Direction", 2);
        }
        else if (vertical < 0)
        {
            animator.SetInteger("Direction", 0);
        }
        else if (horizontal > 0)
        {
            animator.SetInteger("Direction", 1);
        }
        else if (horizontal < 0)
        {
            animator.SetInteger("Direction", 3);
        }
    }
}
