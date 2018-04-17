using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceCrystal : Actor {

	private Fairy_Queen queen;


	//intent is for this to not die, but still visibly take damage
	override public void ActorStart(){
		maxHealth = 9999;
		currentHealth = maxHealth;
		canBeFrozen = false;
		queen = GameObject.FindObjectOfType<Fairy_Queen>();
	}

	//passes damage taken to the fairy queen
	override public void TakeDamage(int amount){
		queen.TakeDamage(amount);
		base.TakeDamage(amount);
		currentHealth = maxHealth; //hehe
	}

}
