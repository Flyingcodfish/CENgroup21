﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Tilemaps;

namespace Pathfinding{

	public class Navigator : MonoBehaviour {

		//objects using the navigator have a specific "blocking type" that describes their interaction with different tilemap layers.
		//walking actors, for example, should be blocked by holes and walls. Flying actors should only be blocked by walls.
		public enum BlockingType{
			walking, flying, ghost
		}

		//see TilemapMatrix class declaration below
		public TilemapMatrix tilemapMatrix;
		public Tilemap background; //the largest tilemap in a scene: used to size node grids
		//stores generated PathMap (value) for each blocking type (key)
		private Dictionary<BlockingType, PathMap> pathMapDict;

		/*
		 * ==== DESCRIPTION ====
		 * 
		 * A Navigator instance is used by any object that wishes to use pathfinding.
		 * The Navigator acts as a mediator between Tilemaps in a scene and pathfinding algorithms,
		 * and makes extensive use of algorithms in the static MapConverter class to perform conversions.
		 * 
		 * The Navigator is used by calling "GetWorldPath".
		 * This provides a suggested path, each entry being a NodeGrid node's world coordinates.
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
		public Vector3[] GetWorldPath(BlockingType bType, Vector3 pointA, Vector3 pointB){
			PathMap pMap = pathMapDict[bType];
			Vector3Int pA_vec = MapConverter.WorldToNode(pMap,pointA);
			Vector3Int pB_vec = MapConverter.WorldToNode(pMap,pointB);
			Point pA = new Point(pA_vec.x, pA_vec.y);
			Point pB = new Point(pB_vec.x, pB_vec.y);
			List<Point> pointList = Pathfinder.FindPath(pMap.nodeGrid, pA, pB);

			Vector3[] coordsList = new Vector3[pointList.Count];
			int i=0;
			pointList.ForEach(delegate (Point p){
				coordsList[i++] = MapConverter.NodeToWorld(pMap, p.x, p.y);
			});
			return coordsList;
		}

		/*
		 * 
		 * ==== HELPER METHODS ====
		 *
		 * Layer interaction shortcuts.
		 * 
		 */

		//returns a mask of layers in which actors are known to reside.
		private static int GetActorMask(){
			return (int)LayerMask.GetMask("Flying") | (int)LayerMask.GetMask("Walking");
		}

		//based on the input blocking type, returns a layer mask used in collision detection.
		public static LayerMask GetMaskFromBlockingType (BlockingType bType, bool includeActors = true){
			int mask;
			switch (bType){
			case BlockingType.walking:
				mask = Physics2D.GetLayerCollisionMask(LayerMask.NameToLayer("Walking"));
				if (includeActors == false) mask = mask & ~GetActorMask();
				break;

			case BlockingType.flying:
				mask = Physics2D.GetLayerCollisionMask(LayerMask.NameToLayer("Flying"));
				if (includeActors == false) mask = mask & ~GetActorMask();
				break;

			default:
			case BlockingType.ghost:
				mask = LayerMask.GetMask("Nothing");
				break;
			}
			return mask;
		}


		//returns a ContactFilter from a blocking type. For convenience.
		public static ContactFilter2D GetFilterFromBlockingType(BlockingType bType, bool includeActors = true){
			ContactFilter2D cFilter = new ContactFilter2D();
			cFilter.layerMask = GetMaskFromBlockingType(bType, includeActors);
			cFilter.useLayerMask = true;
			return cFilter;
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
			pathMapDict[t] =  MapConverter.TilemapArrToPathMap(tilemapMatrix[t].tileMaps, background.cellBounds);
		}


		public void Awake(){
			pathMapDict = new Dictionary<BlockingType, PathMap>();
			//for each list of blocking layers (assumes BlockingType enum starts at 0)
			PathMap pMap;
			for (int t=0; t<tilemapMatrix.lists.Length; t++){
				if (tilemapMatrix.lists[t].tileMaps.Length != 0){
					//generate a PathMap
					pMap = MapConverter.TilemapArrToPathMap(tilemapMatrix[(BlockingType)t].tileMaps, background.cellBounds);
					//add to dictionary, with the current BlockingType as a key
					pathMapDict.Add((BlockingType)t, pMap);
				}
			}
		}


		/*
		 * ==== DATA STRUCTURES ====
		 * 
		 * These structures are used to create a 2-D array of tilemaps.
		 * As such, they form a nesting structure: the matrix holds several lists.
		 * It is done in this way so that tilemaps can easily be assigned to
		 * block different BlockingTypes in the Unity inspector.
		 *
		 * A dictionary would be ideal for this, if it were editable in the inspector.
		 * 
		 */

		//Bottom of the nesting order.
		//Holds a list of tilemaps that block all navogators with the specified BlockingType
		[System.Serializable]
		public struct TilemapList {
			public BlockingType blockingType;
			public Tilemap[] tileMaps;

			public Tilemap this[int i]{
				get{
					return tileMaps[i];
				}
			}
		}

		//Top-level of the nesting order.
		//Indexed by BlockingType, each entry is a BlockList, which is a Tilemap array.
		//each Tilemap in a given BlockingMapList will be considered "blocking" for the corresponding BlockingType.
		[System.Serializable]
		public struct TilemapMatrix{
			public TilemapList[] lists;

			//this is pretty bad, but fortunately it only occurs when (re)generating PathMaps (ie rarely)
			public TilemapList this[BlockingType bType]{
				get{
					//gives precedent to the first list found of the specified blocking type. ideally there shouldn't be duplicates, anyways
					for (int t=0; t < lists.Length; t++){
						if (lists[t].blockingType == bType)
							return lists[t];
					}
					//not found. Whine and return garbage.
					Debug.Log(System.String.Format("Warning: TilemapList for BlockingType '{0}' not found in Navigator TilemapMatrix.", bType));
					return new TilemapList();
				}
			}
		}

	}//end Navigator class

}