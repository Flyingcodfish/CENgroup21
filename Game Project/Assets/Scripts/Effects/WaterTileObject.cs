using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTileObject : MonoBehaviour {
	private Color blueTint = new Color32(198, 243, 243, 0);

	public Sprite liquidSprite;
	public Sprite frozenSprite;

	private Collider2D col;
	private SpriteRenderer spriteRenderer;

	public int stoodOnCount = 0;
	public int frozenCount = 0;
	private float thawTime = 4f;
	private float iceAlpha = 1f;

	public void Start(){
		col = this.GetComponent<Collider2D>();
		spriteRenderer = this.GetComponent<SpriteRenderer>();
		spriteRenderer.color = this.blueTint;
		spriteRenderer.sprite = liquidSprite;
	}


	public void Freeze(){
		StartCoroutine(FreezeAndAnimate());
	}

	IEnumerator FreezeAndAnimate(){
		//animation fields
		Color sColor = spriteRenderer.color;
		float duration = 0.5f;
		float frameRate = 30f;
		float step = iceAlpha / (duration*frameRate);

		bool firstToFreeze = (frozenCount == 0);
		frozenCount++;

		if (firstToFreeze){
			//become walkable immediately
			spriteRenderer.sprite = frozenSprite;
			col.isTrigger = true;
			//animate freezing
			//if alpha goes over 1 it's treated as 1; ensures full visibility by end
			for (float a = sColor.a; a < iceAlpha; a+= step){
				sColor.a = a;
				spriteRenderer.color = sColor;
				yield return new WaitForSeconds(1/frameRate);
			}
		}


		yield return new WaitForSeconds(thawTime);

		//wait for actors to step off before thawing; matter of convenience
		while (stoodOnCount > 0) yield return null;
		frozenCount --;

		//thaw
		if (firstToFreeze){
			while (frozenCount > 0)//wait for other freeze effects to end
				yield return null;

			col.isTrigger = false;

			//animate thawing
			//if alpha goes under 0 it's treated as 0; ensures full transparency by end
			for (float a = iceAlpha; a > 0; a -= step){
				if (frozenCount > 0) break; //can be re-frozen mid thaw animation
				sColor.a = a;
				spriteRenderer.color = sColor;
				yield return new WaitForSeconds(1/frameRate);
			}
			if (frozenCount == 0)spriteRenderer.sprite = liquidSprite;
		}
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
