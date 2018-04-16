using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public abstract class AI_Actor : Actor {

	static float tickTime = 0.1f; //time in seconds between each AI tick

	//targeting & movement fields: useable by inheritors
	protected GameObject targetObject;
	public Vector3 moveVector;
	protected Vector2 directMove;
	public float hoverDistance = 1.2f;
	public float moveDeadZone = 0.1f;

	//navigation fields: usable by inheritors
	protected Navigator navigator;
	public Navigator.BlockingType bType = Navigator.BlockingType.walking;
	protected Vector3[] path;

	//obstacle avoidance fields: while obstacle avoidance should never need to be implemented differently, some fields are protected for use in line of sight calculation
	public float avoidDistance = 1.5f;
	Vector3 avoidVector;
	Vector2 hitDirection;
	int maxHits = 1;
	int numHits;
	protected RaycastHit2D[] castHits;
	protected ContactFilter2D obstacleFilter; //sees terrain AND actors
	protected ContactFilter2D tileFilter; //only sees terrain
	public Collider2D castCollider;


	//sets important references
	override public void ActorStart(){
		targetObject = GameObject.FindWithTag("Player");
		navigator = GameObject.FindWithTag("Navigator").GetComponent<Navigator>();

		obstacleFilter = Navigator.GetFilterFromBlockingType(bType, true);
		tileFilter = Navigator.GetFilterFromBlockingType(bType, false);
		castHits = new RaycastHit2D[maxHits];
		StartCoroutine (AI_Tick ());
		lockAI = 1; //locked by default until we enter aggro range
		moveVector = Vector2.zero;
		this.AI_Start ();
	}

	/*
	 * 
	 * ==== VIRTUAL METHODS BEGIN ====
	 * Everything in the following category can be (or must be) overridden.
	 * Allows the custom tailoring of pathing behavior, but also provides some generally suitable defaults.
	 * 
	 * 
	 */

	//MUST be overridden in inheriting classes; prevents people (like me) from accidentally (and erroneously) overriding ActorStart
	abstract public void AI_Start ();

	virtual protected void OnInHoverDistance (){
		//no need to get closer, stop moving
		moveVector = Vector3.zero;
	}

	virtual protected void OnDirectMove (){
		moveVector = directMove;
	}

	virtual protected void OnPathFind(){
		path = navigator.GetWorldPath(bType, transform.position, targetObject.transform.position);
		if (path.Length > 0){
			moveVector = (path[0]-transform.position).normalized;
		}
		else moveVector = Vector3.zero;
	}

	virtual protected void OnTickEnd(){
		//pass
	}

	/*
	 * 
	 * ==== AI UTILITY CODE ====
	 * Every AI entity sohuld "think" several times a second, but not necessarily every frame.
	 * This is done with a coroutine: AI_Tick. AI_Tick does some pathing stuff,
	 * calling methods depending on the case. Then, OnTickEnd is called, allowing inheritors
	 * to do whatever else they might want to do in a tick.
	 * 
	 * Most AI code should go into OnTickEnd rather than Update, as it's less performance-intensive,
	 * and will also pause when the actor is frozen (or similiarly disabled).
	 * 
	 * 
	 */

	IEnumerator AI_Tick(){
		while (true){
			if (this.IsActive()){
				directMove = targetObject.transform.position - transform.position;

				if (directMove.magnitude <= hoverDistance){
					OnInHoverDistance ();
				}
				//check if we can run straight towards player
				else if (0 == castCollider.Cast(directMove, tileFilter, castHits, directMove.magnitude)){
					OnDirectMove ();
				}
				//pathfinding; only if we must
				else {
					OnPathFind ();
				}

				AvoidObstacles ();
				OnTickEnd ();
			}
			yield return new WaitForSeconds(tickTime);
		}
	}

	//local obstacle avoidance; changes moveVector to avoid unexpected obstacles (generally rigidbodies that aren't part of a tilemap)
	protected void AvoidObstacles(){
		//avoid local obstacles
		numHits = castCollider.Cast((Vector2)moveVector, obstacleFilter, castHits, avoidDistance);

		//if we would run into something if we kept moving forward, and it isn't our target
		if (numHits > 0 && !castHits[0].collider.gameObject.Equals(targetObject)){
			//add a force that is perpendicular to the path we take to hit an obstacle.
			//This moves simultaneously away from the obstacle, and towards our goal.
			hitDirection = castHits[0].point - ((Vector2)transform.position + castCollider.offset);
			float phi = Vector2.SignedAngle(moveVector, hitDirection);
			float sign = Mathf.Sign(phi); //determines which of two perpendicular paths to take
			avoidVector = new Vector2(sign*hitDirection.y, -1*sign*hitDirection.x); //calculate normal to hitVector
			moveVector = avoidVector.normalized + moveVector.normalized;
		}
	}

}
