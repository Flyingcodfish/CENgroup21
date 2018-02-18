using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//base class for anything that breathes (and then some)
public abstract class Actor : MonoBehaviour {

	public int maxHealth;
	protected int currentHealth; //damage must be dealt via message; triggers takeDamage method
	public Team team = Team.enemy; //enemy is default team

	//inheritors should call this
	public virtual void Start(){
		currentHealth = maxHealth;
	}

	public int getHealth(){
		return currentHealth;
	}

	public virtual void takeDamage(int amount){
		currentHealth -= amount;
		if (currentHealth <= 0 ) die();
	}

	public virtual void die(){
		Destroy(this);
	}
}

//makes AI behaviors more flexible/powerful
public enum Team {player, enemy, special};