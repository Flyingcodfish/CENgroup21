using UnityEngine;
using UnityEngine.Tilemaps;

//Object tile. Has an associated gameObject that spawns at this tile's location.
//Useful for when placing things that should sort, collide, or otherwise behave as more than just tiles: trees and doors, for instance.
[CreateAssetMenu(fileName = "New Object Tile", menuName = "Object Tile")]
public class ObjectTile : TileBase {

	//gameObject to spawn
	public GameObject spawnedPrefab;
	public Vector3 offset = new Vector3(0.5f, 0.5f, 0); //(0.5, 0.5, 0) moves object to center of tile

	//tile related fields
	private TileFlags flags = TileFlags.InstantiateGameObjectRuntimeOnly;
	public Tile.ColliderType colliderType = Tile.ColliderType.Sprite;

	//return sprite information when asked
	public override void GetTileData(Vector3Int location, ITilemap tileMap, ref TileData tileData){

		tileData.sprite = spawnedPrefab.GetComponent<SpriteRenderer>().sprite;
		tileData.gameObject = spawnedPrefab;

		tileData.flags = this.flags;
		tileData.colliderType = this.colliderType;
	}

	//sets gameObject position. HideFlags stuff prevents unity from being dumb
	override public bool StartUp(Vector3Int location, ITilemap tilemap, GameObject instance){
		this.hideFlags = HideFlags.None;

		if (instance != null){
            instance.GetComponent<SpriteRenderer>().material = tilemap.GetComponent<TilemapRenderer>().material;
			instance.transform.position += offset;
			instance.hideFlags = HideFlags.DontSaveInEditor;
		}
		return true;
	}
}
