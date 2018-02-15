using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pixelPerfect : MonoBehaviour {

	private Camera cam;

	private int magnification = 2;
	private int PPU = 16;

	//source: https://blogs.unity3d.com/2015/06/19/pixel-perfect-2d/
	void Start () {
		cam = gameObject.GetComponent<Camera>();
		cam.orthographicSize = 0.5f*Display.main.renderingHeight/(magnification*PPU);
	}
}
