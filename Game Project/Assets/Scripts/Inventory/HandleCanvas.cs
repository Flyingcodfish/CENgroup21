using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandleCanvas : MonoBehaviour {// used to scale the Hud for different screen sizes 

    private CanvasScaler scaler;

	// Use this for initialization
	void Start () {
        scaler = GetComponent<CanvasScaler>();

        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
	}
	
}
