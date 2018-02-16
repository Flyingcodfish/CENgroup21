using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : Actor {
	private GameObject target;

	public override void Start(){
		base.Start();
		target = GameObject.FindWithTag("Player");

	}


	public void Update(){
		//



	}

}
