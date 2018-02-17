using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System; //for String
using UnityEngine.Tilemaps; //for TileMap
using NesScripts.Controls.PathFind; //for PathGrid

//struct for carrying walkable tiles, PathGrid, and information to locate it in the world, BoundsInt
public struct PathMap{
	public Tilemap referenceTilemap;
	public BoundsInt bounds;
	public PathGrid pathGrid;
}

//for converting tilemaps to other forms for use in 2D pathfinding; and the reverse
public class MapConverter {

	//converts a group of tilemaps into a matrix of booleans, for use with 2D pathfinding.
	//tilemaps are "flattened" into one: each boolean matrix is logically OR'd together
	//matrix size is determined by tilemap BoundsInt. Uses largest x and y dimensions among maps.
	//Cells with tiles are not walkable, and convert to false. Cells without tiles convert to true.
	public static PathMap TilemapListToPathMap (Tilemap[] maps){
		if (maps == null || maps.Length == 0){
			Debug.Log("Error: maps is invalid");
			return new PathMap(); //TODO: make this return a completely walkable map
		}
		Debug.Log(String.Format("maps is of length: {0}", maps.Length));
		//find largest x, y map dimensions
		BoundsInt outerBounds = new BoundsInt(); //matrix elements are hereby referred to as "nodes"
		BoundsInt[] bounds = new BoundsInt[maps.Length];
		for (int m = 0; m < maps.Length; m++){
			bounds[m] = maps[m].cellBounds;
			Debug.Log(String.Format("bounds {0}: {1}; zMin: {2}, zMax: {3}", m, bounds[m], bounds[m].zMin, bounds[m].zMax));

			outerBounds.zMin = 0;
			outerBounds.zMax = 1;

			if (bounds[m].xMin < outerBounds.xMin)
				outerBounds.xMin = bounds[m].xMin;

			if (bounds[m].xMax > outerBounds.xMax)
				outerBounds.xMax = bounds[m].xMax;

			if (bounds[m].yMin < outerBounds.yMin)
				outerBounds.yMin = bounds[m].yMin;

			if (bounds[m].yMax > outerBounds.yMax)
				outerBounds.yMax = bounds[m].yMax;
		}
		int i, j; //preallocate matrix indices; x/y are for map coords, i/j for matrix indices ("nodes")
		//create bool matrix of proper size
		bool[,] boolMat = new bool[outerBounds.size.x,outerBounds.size.y];
		for (i=0; i<outerBounds.size.x; i++){
			for (j=0; j<outerBounds.size.y; j++){
				boolMat[i,j] = true; //nodes are walkable (true) by default
			}
		}
		//for each map:
		for (int m=0; m<maps.Length; m++){
			Debug.Log(outerBounds);
			//loop through positions, starting at bottom left x/y in map
			//NOTE: MAX BOUNDS ARE EXCLUSIVE
			for (int x = bounds[m].xMin; x < bounds[m].xMax; x++){
				for (int y = bounds[m].yMin; y < bounds[m].yMax; y++){
					//convert tilemap x/y into node i/j
					i = x - outerBounds.xMin;
					j = y - outerBounds.yMin;
					//set matrix bool value: null => true, else false.
					//each map is added together: if ANY map has a tile in a location, that node is false (ie not walkable)
					//Debug.Log(String.Format("i: {0}, j: {1}, x: {2}, y: {3}, tile: {4}", i, j, x, y, maps[m].GetTile(new Vector3Int(x,y,0))));
					boolMat[i,j] = (boolMat[i,j] && (maps[m].GetTile(new Vector3Int(x,y,0)) == null));
				}//y loop
			}//x loop
		}//m loop

		//DEBUG BLOCK
		for (i=0; i<outerBounds.size.x; i++){
			for (j=0; j<outerBounds.size.y; j++){
				Debug.Log(String.Format("i: {0}, j: {1}, tile: {2}", i, j, boolMat[i,j]));
			}
		}
		//END DEBUG

		//return results
		PathMap pathMap = new PathMap();
		pathMap.referenceTilemap = maps[0]; //all tilemaps passed in should share a parent grid layout
		pathMap.bounds = outerBounds;
		pathMap.pathGrid = new PathGrid(outerBounds.size.x,outerBounds.size.y,boolMat);
		return pathMap;
	}
		
	//based on bounds information, and matrix position, returns the TileMap location of a matrix tile
	//might not ever be used except when getting world coordinates, as in "NodeToWorld"
	public static Vector3Int NodeToTile(PathMap pathMap, int i, int j){
		return new Vector3Int(pathMap.bounds.xMin + i, pathMap.bounds.yMin + j, 0);
	}

	public static Vector3Int TileToNode(PathMap pathMap, int x, int y){
		return new Vector3Int(x - pathMap.bounds.xMin, y - pathMap.bounds.yMin, 0);
	}

	//finds the world coordinates of a given node. the basis of all AI behavior
	public static Vector3 NodeToWorld(PathMap pathMap, int i, int j){
		return pathMap.referenceTilemap.GetCellCenterWorld(NodeToTile(pathMap, i, j));
	}

	public static Vector3Int WorldToNode(PathMap pathMap, Vector3 pos){
		Vector3Int point = pathMap.referenceTilemap.WorldToCell(pos);
		return TileToNode(pathMap, point.x, point.y);
	}
}
