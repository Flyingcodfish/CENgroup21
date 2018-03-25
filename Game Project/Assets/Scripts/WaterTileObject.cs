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
	private int stoodOnCount = 0;

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
		col.isTrigger = true;
		StartCoroutine(Thaw());
	}


	IEnumerator Thaw(){
		yield return new WaitForSeconds(thawTime);
		//wait for actors to step off before thawing; matter of convenience
		while (stoodOnCount > 0) yield return null;

		//TODO: some animation
		spriteRenderer.sprite = liquidSprite;
		col.isTrigger = false;
	}


	void OnTriggerEnter2D(Collider2D coll){
		if (coll.gameObject.GetComponent<Actor>() != null)
			stoodOnCount++;
	}

	void OnTriggerExit2D(Collider2D coll){
		if (coll.gameObject.GetComponent<Actor>() != null)
			stoodOnCount--;
	}

}
