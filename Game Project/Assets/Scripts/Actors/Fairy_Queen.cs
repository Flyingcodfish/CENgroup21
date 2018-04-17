using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fairy_Queen : MonoBehaviour {

	public Projectile circleShotPrefab;
	private Projectile circleShotInstance;

	void Update(){
		if (Input.GetKeyDown(KeyCode.F)){
			//fire circle shot
			circleShotInstance = Instantiate(circleShotPrefab, transform.position, Quaternion.identity);
			circleShotInstance.Initialize(0.05f * Vector2.right, Team.enemy);
		}
	}
}
