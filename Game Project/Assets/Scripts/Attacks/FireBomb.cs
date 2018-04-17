using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TeamComponent))]
public class FireBomb : MonoBehaviour {

    public TeamComponent teamComponent;
    public Vector2 velocity;
    public int damage = 30;
    private float explodetimer = 3.0f;
    private float stoptimer = 0.5f;
    public Sprite littorch;
    public Animator animator;

    public Explosion explosionPrefab;

    public void OnTriggerEnter2D(Collider2D other)
    {
        //we hit something. if it is a wall, or on another team, "hit" it and destroy the bullet.
		TeamComponent otherTeam = other.GetComponent<TeamComponent>();

        if (other.tag == "Torch")
        {
            other.GetComponent<SpriteRenderer>().sprite = littorch;
            GameObject lightObject = new GameObject("Torch light");
            lightObject.transform.position = new Vector3(other.transform.position.x, other.transform.position.y, -1);
            Light light = lightObject.AddComponent<Light>();
            light.intensity = 10;
            light.range = 40;
            light.renderMode = LightRenderMode.ForcePixel;
            other.tag = "LitTorch";
            other.GetComponent<SpriteRenderer>().color = UnityEngine.Color.white;
        }
		else if (otherTeam == null || otherTeam.team != this.teamComponent.team)
        {
            //nicely ask the target to take damage
			other.gameObject.SendMessage("TakeDamage", this.damage, SendMessageOptions.DontRequireReceiver);
            this.Die();
        }
        //else ignore the collision
    }

    private void Die()
    {
        //probably hit a wall or target; destroy this bullet
        //instantiate a fade-out effect/object
        Explosion exp = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
		exp.damage = this.damage;
		Destroy(this.gameObject);
    }

    public void Initialize(Vector2 velocity, Team team, float dmgMod = 1f)
    {
        animator = this.GetComponent<Animator>();
        this.velocity = velocity;
		this.teamComponent = this.GetComponent<TeamComponent>();
		teamComponent.team = team;
		this.damage = (int) (this.damage * dmgMod);
		this.gameObject.layer = LayerMask.NameToLayer("Projectiles");
        if (velocity.y > 0)
        {   //up
            animator.SetInteger("Direction", 0);
        }
        else if (velocity.y < 0)
        { //down
            animator.SetInteger("Direction", 1);
        }
        else if (velocity.x < 0)
        { //left
            animator.SetInteger("Direction", 2);
        }
        else if (velocity.x > 0)
        { //right
            animator.SetInteger("Direction", 3);
        }
        else
        {
            //leave facing as is
        }
    }

    public void FixedUpdate()
    {
        transform.position = transform.position + (Vector3)velocity;
        stoptimer -= Time.deltaTime;
        explodetimer -= Time.deltaTime;
        if (stoptimer <= 0)
            this.velocity = new Vector2(0.0f, 0.0f);
        if (explodetimer <= 0)
            this.Die();
    }
}
