﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Tilemaps;

//modified from: https://www.youtube.com/watch?v=ft4HUL2bFSQ
//applies a shadow to a tilemap.
//[ExecuteInEditMode]
[RequireComponent(typeof(TilemapRenderer))]
public class TilemapShadow : MonoBehaviour {

	public Vector3 offset = new Vector3(-0.1f, 0.1f, 0); //negative y offset breaks physics

	private TilemapRenderer casterRenderer;
	private TilemapRenderer shadowRenderer;

	private GameObject shadowObject;

	public Material shadowMaterial;


	// Use this for initialization
	void Start () {
		shadowObject = Instantiate(this.gameObject, offset, Quaternion.identity);
		DestroyImmediate(shadowObject.GetComponent<TilemapShadow>()); //we really don't want recursion here
		shadowObject.name = this.name + " (Shadow)";
		shadowObject.transform.parent = this.transform.parent;
		shadowObject.transform.position = offset;

		shadowRenderer = shadowObject.GetComponent<TilemapRenderer>();
		shadowRenderer.material = shadowMaterial;
		shadowRenderer.sortingOrder -= 1;
	}
}
