using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//modified from: https://www.youtube.com/watch?v=ft4HUL2bFSQ
public class DropShadow : MonoBehaviour {

    public Vector3 offset = new Vector3(-0.2f, -0.2f, 0);

    private SpriteRenderer sCaster;
    private SpriteRenderer sShadow;

    private Transform tCaster;
    private Transform tShadow;

    public Color shadowColor;


    // Use this for initialization
    void Start () {
        tCaster = transform;
        tShadow = new GameObject().transform;
        tShadow.parent = tCaster;
        tShadow.gameObject.name = "shadow";

		tShadow.localScale = Vector3.one;
		tShadow.localPosition = offset;

        sCaster = GetComponent<SpriteRenderer>();
        sShadow = tShadow.gameObject.AddComponent<SpriteRenderer>();

        sShadow.color = shadowColor;
        sShadow.sortingLayerName = sCaster.sortingLayerName;
        sShadow.sortingOrder = sCaster.sortingOrder-1;
        }


    void LateUpdate()
    {
        sShadow.sprite = sCaster.sprite;
		sShadow.flipX = sCaster.flipX;
    }

}
