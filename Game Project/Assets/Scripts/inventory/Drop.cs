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

	private int keyID;

	void Start(){
		if (dropType == DropType.KEY) {
			//generate key hash; use to uniquely ID it and save if it's been picked up
			//this process consists of randome garbage bitwise operations, using location and the scene's name to uniquely identify this key.
			//Due to the nature of the below operations, as long as a key/scene doesn't move/change names, its ID will evaluate to the same value every time.
			long hash = (int) (2*transform.position.x);
			hash = (hash + 0x00C0FFEE) + (hash << 11);
			hash = (hash + 0x0EC50DEE) ^ (hash >> 15);
			hash ^= (int) (2*transform.position.y);
			hash = (hash + 0x0ABC1230) + (hash << 9);
			hash = (hash + 0x420B1A5E) ^ (hash << 12);

			string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene ().name;
			Random.InitState((int)hash);
			hash ^= (char) sceneName[Random.Range (0, sceneName.Length)]; //assumes no scene has an empty name :^)
			hash ^= (char) sceneName[Random.Range (0, sceneName.Length)];
			keyID = (int) hash;

			//if this item is a key and has been picked up before, just pretend it doesn't exist
			if (GameSaver.liveSave.pickedUpKeys.Contains (keyID))
				Destroy (this.gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		PlayerControl player = other.gameObject.GetComponentInParent<PlayerControl>(); //allows player attack hitboxes to grab drops
		if (player != null){
			switch (this.dropType) {
			case DropType.KEY:
				player.hasKeys += 1;
				GameSaver.liveSave.pickedUpKeys.Add (keyID); //note that we've been picked up
				break;
			case DropType.HEALTH:
				player.TakeDamage (-healAmount);
				break;
			case DropType.MANA:
				player.SpendMana (-manaAmount);
				break;
				//TODO add cases for money drops; add money fields and HUD elements to PlayerControl
			}
			Destroy (this.gameObject);
		}
	}

}
