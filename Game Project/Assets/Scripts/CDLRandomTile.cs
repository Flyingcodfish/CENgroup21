using UnityEngine;
using UnityEngine.Tilemaps;

//Random tile. When placed into a tilemap, will become one random sprite from its array.
//Useful for large, flat tilemap regions that benefit from some variety.
//Does not change after placement, probably :^)
[CreateAssetMenu(fileName = "New CDL Random Tile", menuName = "CDL Random Tile")]
public class CDLRandomTile : TileBase {

	//array of choices
	public Sprite[] sprites;

	//add fields to imitate a normal sprite
	public Color color = Color.white;
	private Matrix4x4 transform = Matrix4x4.identity;
	private GameObject gameObject = null;
	private TileFlags flags = TileFlags.LockColor;
	public Tile.ColliderType colliderType = Tile.ColliderType.Sprite;

	//return a random sprite from the array of choices
	public override void GetTileData(Vector3Int location, ITilemap tileMap, ref TileData tileData){

		int randI = (int)Random.Range(0, sprites.Length);
		tileData.sprite = sprites[randI];

		tileData.color = this.color;
		tileData.transform = this.transform;
		tileData.gameObject = this.gameObject;
		tileData.flags = this.flags;
		tileData.colliderType = this.colliderType;
	}
}
