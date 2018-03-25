using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Collider2D))]
public class SceneTrigger: MonoBehaviour {
    public string SceneName;

	public void OnTriggerEnter2D(Collider2D other){
		UnityEngine.SceneManagement.SceneManager.LoadScene(SceneName);
	}

}
