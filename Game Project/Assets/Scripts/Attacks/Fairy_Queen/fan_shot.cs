using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fan_shot : Projectile {

	public float flyTime;
	public int spreadCount;
	public float spreadRange; //in degrees

	public Projectile straightShotPrefab;
	private Projectile straightShotInstance;

	public float baseSpeed;

	override public void OnInit(){
		StartCoroutine(SpreadTimer());
		baseSpeed = velocity.magnitude;
	}

	IEnumerator SpreadTimer(){
		yield return new WaitForSeconds (flyTime);

		//create a series of spread-out shots, then die
		for (float angle = -spreadRange/2; angle <= spreadRange/2; angle += spreadRange/(spreadCount-1)){
			straightShotInstance = Instantiate(straightShotPrefab, transform.position, Quaternion.identity);
			straightShotInstance.transform.up = velocity; //point forward, initially
			straightShotInstance.transform.eulerAngles += new Vector3(0, 0, angle); //rotate according to spread
			//Debug.Log("firing spread shot. Velocity: " + baseSpeed + "in direction: " + straightShotInstance.transform.up.ToString());
			straightShotInstance.Initialize(straightShotInstance.transform.up * baseSpeed, Team.enemy); //launch in new direction
		}
		Die();
	}

}
