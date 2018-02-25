using UnityEngine;
using UnityEngine.Tilemaps;

//Object tile. Has an associated gameObject that spawns at this tile's location.
//Useful for when placing things that should sort, collide, or otherwise behave as more than just tiles: trees and doors, for instance.
[CreateAssetMenu(fileName = "New Object Tile", menuName = "Object Tile")]
public class ObjectTile : TileBase {

	//representative sprite
	public Sprite sprite;

	//gameObject to spawn
	public GameObject spawnedPrefab;
	public Vector3 offset = new Vector3(0.5f, 0.5f, 0); //(0.5, 0.5, 0) moves object to center of tile

	//add fields to imitate a normal tile
	public Color color = Color.white;
	private Matrix4x4 transform = Matrix4x4.identity;
	private TileFlags flags = TileFlags.LockColor;
	public Tile.ColliderType colliderType = Tile.ColliderType.Sprite; //TODO mess with this; collision might be from gameObject spawned

	//return sprite information when asked
	public override void GetTileData(Vector3Int location, ITilemap tileMap, ref TileData tileData){
		tileData.sprite = this.sprite;
		
		tileData.gameObject = spawnedPrefab;

		tileData.color = this.color;
		tileData.transform = this.transform;
		tileData.flags = this.flags;
		tileData.colliderType = this.colliderType;
	}

	override public bool  StartUp(Vector3Int location, ITilemap tilemap, GameObject gObject){
		if (gObject != null)
			gObject.transform.position = gObject.transform.position + offset;

		return base.StartUp(location, tilemap, gObject);
	}
}
