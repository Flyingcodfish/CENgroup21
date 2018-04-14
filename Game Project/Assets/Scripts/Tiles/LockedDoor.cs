using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : MonoBehaviour {

	public Sprite closedSprite;
	public Sprite openSprite;

	public SpriteRenderer doorwayRenderer;

	private SpriteRenderer sRenderer;
	private Collider2D coll;

	private int doorID;

	// Use this for initialization
	void Start () {
		sRenderer = this.GetComponent<SpriteRenderer> ();
		coll = this.GetComponent<Collider2D> ();
		doorwayRenderer.material = sRenderer.material;

		//generate door hash; use to uniquely ID it and save if it's been unlocked
		//this process consists of randome garbage bitwise operations, using location and the scene's name to uniquely identify this door.
		//Due to the nature of the below operations, as long as a door/scene doesn't move/change names, its ID will evaluate to the same value every time.
		long hash = (int)(2 * transform.position.x);
		hash = (hash + 0xBAEBAEBA) + (hash << 16);
		hash = (hash + 0x01173430) ^ (hash >> 12);
		hash ^= (int)(2 * transform.position.y);
		hash = (hash + 0x1EA60E01) + (hash << 12);
		hash = (hash + 0x240417AC) ^ (hash << 9);

		string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene ().name;
		Random.InitState ((int)hash);
		hash ^= (char)sceneName[Random.Range (0, sceneName.Length - 1)];
		hash ^= (char)sceneName[Random.Range (0, sceneName.Length - 1)];
		doorID = (int)hash;

		//if this item is a key and has been picked up before, just pretend it doesn't exist
		if (GameSaver.liveSave.unlockedDoors.Contains (doorID))
			Open ();
	}



	void OnCollisionEnter2D(Collision2D coll){
		PlayerControl player = coll.gameObject.GetComponent<PlayerControl>();
		if (player != null && player.hasKeys > 0){
			player.hasKeys -= 1;
			GameSaver.liveSave.unlockedDoors.Add (doorID);
			Open ();
		}
	}


	public void Open(){
		this.sRenderer.sprite = openSprite;
		this.coll.enabled = false;
		this.doorwayRenderer.enabled = true;
	}
}

