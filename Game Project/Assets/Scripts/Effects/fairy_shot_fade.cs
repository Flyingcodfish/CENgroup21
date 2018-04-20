using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//shouldn't really have to require this, everything's a sprite, but whatever
[RequireComponent(typeof(SpriteRenderer))]
public class fairy_shot_fade : MonoBehaviour {

	private SpriteRenderer spriteRenderer;
	private Color spriteColor;

	// Use this for initialization
	void Start () {
		this.spriteRenderer = this.GetComponent<SpriteRenderer>();
		spriteColor = spriteRenderer.color;
		StartCoroutine(Fadeout());
	}
	
	IEnumerator Fadeout(){
		float maxSize = 1.5f;
		float minSize = 0f;
		float duration = 0.15f; //duration of animation in seconds
		float fadeFramerate = 30; //Hz

		//in units:  (scale-scale) / (seconds * frames/second) => scale/frame
		float step = (maxSize-minSize) / (duration*fadeFramerate);

		for (float flashSize = minSize; flashSize <= maxSize; flashSize += step){
			spriteColor.a= 1f - ((flashSize-minSize) / (maxSize-minSize)); //fade out as size increases
			spriteRenderer.color = spriteColor;
			this.transform.localScale = Vector3.one * flashSize; //decrease in size
			yield return new WaitForSeconds(1/fadeFramerate);
		}
		Destroy(this.gameObject);
	}
}
