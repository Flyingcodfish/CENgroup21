using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DropType{
	NONE, KEY, HEALTH, MANA, MONEY_S, MONEY_M, MONEY_L
}

[RequireComponent(typeof(Collider2D))]
public class Drop : MonoBehaviour {

	public DropType dropType;
	static int healAmount = 10;
	static float manaAmount = 10f;

	void OnTriggerEnter2D(Collider2D other){
		PlayerControl player = other.gameObject.GetComponentInParent<PlayerControl>(); //allows player attack hitboxes to grab drops
		if (player != null){
			switch (this.dropType) {
			case DropType.KEY:
				player.hasKeys += 1;
				break;
			case DropType.HEALTH:
				player.TakeDamage (-healAmount);
				break;
			case DropType.MANA:
				player.SpendMana (-manaAmount);
				break;
				//TODO add cases for money drops; add money fields and HUD elements to playerControl
			}
			Destroy (this.gameObject);
		}
	}

}
