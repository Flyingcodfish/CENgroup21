using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    private Collider2D hitbox;

    public float damage;
    public Team team;
    public Sprite littorch;

    //declares a delegate method type
    public delegate void DelegateHitActor(Actor actor);
    //an instance of the delegate type: can be assigned to by parents
    //a parent can assign a method to this delegate, which will be called whenever the delegate is called
    //allows actors can impose special effects on their targets, or know whether their attacks hit
    public DelegateHitActor HitActor;

    public void Start()
    {
        this.hitbox = this.GetComponent<Collider2D>();
        Destroy(this.gameObject, 1.0f);
    }

    //attack hitbox hit something
    public void OnTriggerEnter2D(Collider2D other)
    {
        Actor hitActor = other.gameObject.GetComponent<Actor>();

        if (other.tag == "Torch")
        {
            other.GetComponent<SpriteRenderer>().sprite = littorch;
        }

        if (hitActor != null && hitActor.team != this.team)
        {
            //nicely ask the target to take damage
            if (HitActor != null)
                HitActor(hitActor);
            hitActor.SendMessage("TakeDamage", this.damage);
        }
        //else ignore the collision
    }
}
