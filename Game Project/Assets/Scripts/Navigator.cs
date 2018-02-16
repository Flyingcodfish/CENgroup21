using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Tilemaps;
using System; //for Enum static methods

public class Navigator : MonoBehaviour {
	public enum BlockingType{
		walking, flying, ghost
	}

	//Each row index is a BlockingType, each row entry is a Tilemap array.
	//each Tilemap in a given row will be considered "blocking" for the corresponding behavior.
	//walking, for example, should be blocked by holes and walls. Flying should only be blocked by walls.
	public Tilemap[][] blockingMaps = new Tilemap[Enum.GetValues(typeof(BlockingType)).Length][];

	//stores generated PathMap (entry) for each blocking type (index)
	private PathMap[] pathMaps = new PathMap[Enum.GetValues(typeof(BlockingType)).Length];


	/// <summary>
	/// Get a world location to walk towards, in order to reach the desired destination.
	/// </summary>
	/// <param name="pointA">Entity's current world location.</param></param>
	/// <param name="PointB">World location of final destination.</param>
	public Vector3 GetDest(Vector3 pointA, Vector3 pointB){
		return Vector3.zero;
	}

	//regenerates pathMap, accounting for changes in blocking tilemaps.
	//probably unnecessary after Start()
	public void RefreshPathMap(){
		//pass
	}

	public void Start(){
		//for each blocking type:
		RefreshPathMap();
	}
}
