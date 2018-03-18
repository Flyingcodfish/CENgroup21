using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTileObject : MonoBehaviour {
	private Color color = new Color32(198, 243, 243, 255);

	public Sprite liquidSprite;
	public Sprite frozenSprite;

	private float thawTime = 4f;

	private Collider2D col;
	private SpriteRenderer spriteRenderer;
	//TODO: DEBUG; handle actors standing on thawing platforms
	[SerializeField]
	private bool isStoodOn;

	public void Start(){
		col = this.GetComponent<Collider2D>();
		spriteRenderer = this.GetComponent<SpriteRenderer>();
		spriteRenderer.color = this.color;
		spriteRenderer.sprite = liquidSprite;
	}


	public void Freeze(){
		//turn into a platform
		//TODO: some sort of animation
		spriteRenderer.sprite = frozenSprite;
		col.enabled = false;
		StartCoroutine(Thaw());
	}


	IEnumerator Thaw(){
		yield return new WaitForSeconds(thawTime);
		//wait for actors to step off before thawing; matter of convenience
		while (isStoodOn == true) yield return null;

		//TODO: some animation
		spriteRenderer.sprite = liquidSprite;
		col.enabled = true;
		//TODO throw actors off if they're standing on me
	}


	void OnCollisionEnter2D(Collision2D coll){
		if (coll.gameObject.GetComponent<Actor>() != null)
			isStoodOn = true;
	}

	void OnCollisionExit2D(Collision2D coll){
		if (coll.gameObject.GetComponent<Actor>() != null)
			isStoodOn = false;
	}

}
