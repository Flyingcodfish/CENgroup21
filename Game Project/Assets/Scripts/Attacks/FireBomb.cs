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

    public Explosion explosionPrefab;

    public void OnTriggerEnter2D(Collider2D other)
    {
        //we hit something. if it is a wall, or on another team, "hit" it and destroy the bullet.
		TeamComponent otherTeam = other.GetComponent<TeamComponent>();

        if (other.tag == "Torch")
        {
            other.GetComponent<SpriteRenderer>().sprite = littorch;
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
        this.velocity = velocity;
		this.teamComponent = this.GetComponent<TeamComponent>();
		teamComponent.team = team;
		this.damage = (int) (this.damage * dmgMod);
		this.gameObject.layer = LayerMask.NameToLayer("Projectiles");
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
