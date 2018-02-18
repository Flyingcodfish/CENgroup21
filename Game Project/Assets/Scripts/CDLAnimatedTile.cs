using UnityEngine;
using UnityEngine.Tilemaps;

// Tile that plays an animated loops of sprites.
//modified from: https://docs.unity3d.com/ScriptReference/Tilemaps.TileBase.GetTileAnimationData.html
[CreateAssetMenu(fileName = "New CDL Animated Tile", menuName = "CDL Animated Tile")]
public class CDLAnimatedTile : TileBase {
	//fields required for an animated sprite.
	//All animation handled by tile renderer and tilemap
	public Sprite[] m_AnimatedSprites;
	public float m_AnimationSpeed = 2f; //in frames per second
	public float m_AnimationStartTime; //offset in seconds

	//add fields to imitate a normal sprite
	public Color color = Color.white;
	private Matrix4x4 transform = Matrix4x4.identity;
	private GameObject gameObject = null;
	private TileFlags flags = TileFlags.LockColor;
	public Tile.ColliderType colliderType = Tile.ColliderType.Sprite;

	//give tile data to tileMap and renderer. 
	public override void GetTileData(Vector3Int location, ITilemap tileMap, ref TileData tileData){
		if (m_AnimatedSprites != null && m_AnimatedSprites.Length > 0){
			tileData.sprite = m_AnimatedSprites[0]; //send the first frame of the animation as a "representative."
													//tilemap palette thumbnail and physics shape are thus determined by this tile.
		}
		else{
			tileData.sprite = null;
		}
		tileData.color = this.color;
		tileData.transform = this.transform;
		tileData.gameObject = this.gameObject;
		tileData.flags = this.flags;
		tileData.colliderType = this.colliderType;
	}

	//base method just returns false; we need to provide information to renderer and return true (success)
	public override bool GetTileAnimationData(Vector3Int location, ITilemap tileMap, ref TileAnimationData tileAnimationData){
		if (m_AnimatedSprites != null && m_AnimatedSprites.Length > 0){
			tileAnimationData.animatedSprites = m_AnimatedSprites;
			tileAnimationData.animationSpeed = m_AnimationSpeed;
			tileAnimationData.animationStartTime = m_AnimationStartTime;
			return true;
		}
		//else
		return false;
	}
}