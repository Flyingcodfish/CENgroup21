using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {
	
    public float damage;
    public Sprite littorch;

    //declares a delegate method type
    public delegate void DelegateHitActor(Actor actor);
    //an instance of the delegate type: can be assigned to by parents
    //a parent can assign a method to this delegate, which will be called whenever the delegate is called
    //allows actors can impose special effects on their targets, or know whether their attacks hit
    public DelegateHitActor HitActor;

    public void Start()
    {
        Destroy(this.gameObject, 1.0f);
    }

	public void OnTriggerEnter2D(Collider2D other)
	{
		//we hit something. We're an explosion, and we hate everyone. deal damage to everyone.
		if (other.tag == "Torch")
		{
			other.GetComponent<SpriteRenderer>().sprite = littorch;
		}
		else
		{
			//nicely ask the target to take damage
			other.gameObject.SendMessage("TakeDamage", this.damage);
		}
		//else ignore the collision
	}
}
