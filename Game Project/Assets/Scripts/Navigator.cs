using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Tilemaps;
using NesScripts.Controls.PathFind;


public class Navigator : MonoBehaviour {

	//objects using the navigator have a specific "blocking type" that describes their interaction with different tilemap layers.
	//walking actors, for example, should be blocked by holes and walls. Flying actors should only be blocked by walls.
	public enum BlockingType{
		walking, flying, ghost
	}

	//see class declaration below
	public BlockingMapMatrix blockingMapMatrix;
	//stores generated PathMap (value) for each blocking type (key)
	private Dictionary<BlockingType, PathMap> pathMaps;

	/*
	 * ==== DESCRIPTION ====
	 * 
	 * A Navigator instance is used by any object that wishes to use pathfinding.
	 * The Navigator acts as a mediator between Tilemaps in a scene and pathfinding algorithms,
	 * and makes extensive use of algorithms in the static MapConverter class to perform conversions.
	 * 
	 * The Navigator is used by calling "GetPath".
	 * This provides a suggested path, each entry being a PathGrid node's world coordinates.
	 * Objects trying to navigate should pass in their current location and their destination,
	 * then attempt to *directly* move to each node in turn (likely moving on to the next node
	 * once arriving within a certain distance of the current node).
	 * Calls should be made roughly every time an object's target destination changes.
	 * This may be very often if chasing a moving object, so effort should be made to limit calls.
	 * 
	 * The Navigator is handy because its inputs and outputs are world coordinates, so objects
	 * have an easy time communicating with it.
	 * 
	 * The Navigator also generates and stores all important pathfinding data (PathMaps and their contents),
	 * so that each object/actor using pathfinding does not need its own copy, which saves memory.
	 * 
	 */


	/// <summary>
	/// Get a world location to walk towards, in order to reach the desired destination.
	/// </summary>
	/// <param name="pointA">Entity's current world location.</param></param>
	/// <param name="PointB">World location of final destination.</param>
	public Vector3[] GetPath(BlockingType bType, Vector3 pointA, Vector3 pointB){
		PathMap pMap = pathMaps[bType];
		Vector3Int pA_vec = MapConverter.WorldToNode(pMap,pointA);
		Vector3Int pB_vec = MapConverter.WorldToNode(pMap,pointB);
		Point pA = new Point(pA_vec.x, pA_vec.y);
		Point pB = new Point(pB_vec.x, pB_vec.y);
		//TODO this might be the worst code I've ever written. This is worse than VHDL type conversion.
		//This is worse than using 4 different graphics adapters in series
		//This is like putting a box into another box because the bigger box has a nice little rectanlge for you to tape the shipping label onto
		//I'm tired and this will be changed soon
		List<Point> pointList = Pathfinding.FindPath(pMap.pathGrid, pA, pB);

		Vector3[] coordsList = new Vector3[pointList.Count];
		int i=0;
		pointList.ForEach(delegate (Point p){
			coordsList[i++] = MapConverter.NodeToWorld(pMap, p.x, p.y);
		});
		return coordsList;
	}

	/*
	 * ==== INITIALIZATION ====
	 * 
	 * A Navigator must generate nodes and grids based on the tilemaps in a scene.
	 * 
	 */

	//regenerates pathMap, accounting for changes in blocking tilemaps.
	//probably unnecessary
	public void RefreshPathMap(BlockingType t){
		pathMaps[t] =  MapConverter.TilemapListToPathMap(blockingMapMatrix[t].blockingMaps);
	}

	public void Start(){
		pathMaps = new Dictionary<BlockingType, PathMap>();
		//for each list of blocking layers (assumes BlockingType enum starts at 0)
		PathMap pMap;
		for (int t=0; t<blockingMapMatrix.blockingLists.Length; t++){
			//generate a PathMap
			pMap = MapConverter.TilemapListToPathMap(blockingMapMatrix[(BlockingType)t].blockingMaps);
			//add to dictionary, with the current BlockingType as a key
			pathMaps.Add((BlockingType)t, pMap);
		}
	}


	/*
	 * ==== DATA STRUCTURES ====
	 * 
	 * The below structures are used to create a 2-D array of tilemaps.
	 * It is done in this way so as to be editable in the Unity inspector.
	 * I personally think this solution is awful, but it works.
	 *
	 * A dictionary would be ideal for this, if it were editable in the inspector.
	 * A better solution would thus be to write a custom editor for a dictionary, but that's beyond me.
	 * 
	 */

	[System.Serializable]
	public class BlockingMapList {
		public BlockingType blockingType;
		public Tilemap[] blockingMaps;

		public Tilemap this[int i]{
			get{
				return blockingMaps[i];
			}
		}
	}

	//holder of lists, made to be editable in the unity inspector.
	//Indexed by BlockingType, each entry is a BlockList, which is a Tilemap array.
	//each Tilemap in a given BlockingMapList will be considered "blocking" for the corresponding BlockingType.
	[System.Serializable]
	public class BlockingMapMatrix{
		public BlockingMapList[] blockingLists;

		//this is pretty slow, fortunately it only occurs when (re)generating PathMaps (ie rarely)
		public BlockingMapList this[BlockingType bType]{
			get{
				//gives precedent to the first list found of the specified blocking type.
				for (int t=0; t < blockingLists.Length; t++){
					if (blockingLists[t].blockingType == bType)
						return blockingLists[t];
				}
				//not found
				return null;
			}
		}
	}
}