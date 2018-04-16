using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Fountain : MonoBehaviour {

	public GameObject iceBlock;
	public Tilemap fountainPool; 
	public Collider2D hole;
	public SpriteRenderer stemBlocker;

	public GameObject keyPrefab;
	private Vector3 keyOffset = new Vector3(0f, -2f, 0f);

	public void Freeze(){
		iceBlock.SetActive(true);
		GetComponent<Animator>().SetTrigger("Freeze");

		StartCoroutine(AnimateDrainWater());
	}

	IEnumerator AnimateDrainWater(){
		//block stem
		stemBlocker.enabled = true;

		//lower water
		float drainDuration = 1.5f; //duration of animation in seconds
		float drainFramerate = 30; //Hz
		//in units:  (scale-scale) / (seconds * frames/second) => scale/frame
		float drainStep = (1f) / (drainDuration*drainFramerate);
		for (float y = 0f; y <= 1f; y += drainStep){
			fountainPool.transform.position -= new Vector3(0f, drainStep, 0f); //move pool downwards, simulation draining
			yield return new WaitForSeconds(1/drainFramerate);
		}

		//fade water
		float fadeDuration = 0.25f; //duration of animation in seconds
		float fadeFramerate = 30; //Hz
		//in units:  (scale-scale) / (seconds * frames/second) => scale/frame
		float fadeStep = (255f) / (fadeDuration*fadeFramerate);
		for (float a = 255f; a >= 0f; a -= fadeStep){
			fountainPool.color = new Color32(198, 243, 243, (byte)a); //blue tint, disappearing
			stemBlocker.color = new Color32 (255, 255, 255, (byte)a);
			yield return new WaitForSeconds(1/fadeFramerate);
		}

		//make pool walkable
		hole.enabled = false;
		//spawn key; will despawn in 1 frame if it's already been picked up. Meh.
		Instantiate(keyPrefab, transform.position + keyOffset, Quaternion.identity);
	}
}
