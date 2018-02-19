using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//modified from: https://www.youtube.com/watch?v=ft4HUL2bFSQ
//[ExecuteInEditMode]
[RequireComponent(typeof(SpriteRenderer))]
public class SpriteShadow : MonoBehaviour {

    public Vector3 offset = new Vector3(-0.2f, 0.2f, 0); //nagative y shadows require the shadow be flippedY and moved downwards

    private SpriteRenderer sCaster;
    private SpriteRenderer sShadow;

    private Transform tCaster;
    private Transform tShadow;

	public Color shadowColor = new Color(0, 0, 0, 0.5f); //values range from 0f to 1f


    // Use this for initialization
    void Start () {
        tCaster = transform;
        tShadow = new GameObject().transform;
        tShadow.parent = tCaster;
		tShadow.gameObject.name = this.name + " (Shadow)";

		tShadow.localScale = Vector3.one;
		tShadow.localPosition = offset;

        sCaster = GetComponent<SpriteRenderer>();
        sShadow = tShadow.gameObject.AddComponent<SpriteRenderer>();

		sShadow.color = shadowColor;
		sShadow.sortingLayerName = "Shadows";
        }


    void LateUpdate()
    {
        sShadow.sprite = sCaster.sprite;
		sShadow.flipX = sCaster.flipX;
    }

}
