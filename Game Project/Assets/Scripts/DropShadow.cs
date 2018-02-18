using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropShadow : MonoBehaviour {

    public Vector2 offset = new Vector2(-3, -3);

    private SpriteRenderer sCaster;
    private SpriteRenderer sShadow;

    private Transform tCaster;
    private Transform tShadow;

    public Material shadowMaterial;
    public Color shadowColor;


    // Use this for initialization
    void Start () {
        tCaster = transform;
        tShadow = new GameObject().transform;
        tShadow.parent = tCaster;
        tShadow.gameObject.name = "shadow";
        //tShadow.localRotation = Quaternion.identity;

        sCaster = GetComponent<SpriteRenderer>();
        sShadow = tShadow.gameObject.AddComponent<SpriteRenderer>();

        sShadow.material = shadowMaterial;
        sShadow.color = shadowColor;
        //sShadow.sortingLayerName = sCaster.sortingLayerName;
        //sShadow.sortingOrder = sCaster.sortingOrder-1;

        }


    void LateUpdate()
    {
        tShadow.position = new Vector2(tCaster.position.x + offset.x,
            tCaster.position.y + offset.y);
        sShadow.sprite = sCaster.sprite;
    }


    //source: https://www.youtube.com/watch?v=ft4HUL2bFSQ
}
