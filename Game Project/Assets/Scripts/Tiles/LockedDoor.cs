using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : MonoBehaviour {

	public Sprite closedSprite;
	public Sprite openSprite;

	public SpriteRenderer doorwayRenderer;

	private SpriteRenderer sRenderer;
	private Collider2D coll;

	// Use this for initialization
	void Start () {
		sRenderer = this.GetComponent<SpriteRenderer>();
		coll = this.GetComponent<Collider2D>();
        doorwayRenderer.material = sRenderer.material;
	}


	void OnCollisionEnter2D(Collision2D coll){
		PlayerControl player = coll.gameObject.GetComponent<PlayerControl>();
		if (player != null && player.hasKeys > 0){
			player.hasKeys -= 1;
			this.sRenderer.sprite = openSprite;
			this.coll.enabled = false;
			this.doorwayRenderer.enabled = true;
		}
	}
}

