﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityEngine.Tilemaps{
	//Random tile. When placed into a tilemap, will become one random sprite from its array.
	//Useful for large, flat tilemap regions that benefit from some variety.
	//Does not change after placement, probably :^)
	[CreateAssetMenu(fileName = "New CDL Random Tile", menuName = "CDL Random Tile")]
	[Serializable]
	public class CDLRandomTile : TileBase {

		//array of choices
		public SpriteAndWeight[] choices;
		private Sprite[] sprites;
		public int totalWeight;
		public bool dirty = false;

		//add fields to imitate a normal sprite
		public Color color = Color.white;
		private Matrix4x4 transform = Matrix4x4.identity;
		private GameObject gameObject = null;
		private TileFlags flags = TileFlags.LockColor;
		public Tile.ColliderType colliderType = Tile.ColliderType.Sprite;

		//return a random sprite from the array of choices
		public override void GetTileData(Vector3Int location, ITilemap tileMap, ref TileData tileData){
			//hash code from https://github.com/Unity-Technologies/2d-extras/blob/master/Assets/Tilemap/Tiles/Random%20Tile/Scripts/RandomTile.cs
			//makes tile choice random, but uses tile location as a seed, thus:
			//tiles will not change every time the scene is updated
			if ((choices != null) && (choices.Length > 0))
			{
				long hash = location.x;
				hash = (hash + 0xabcd1234) + (hash << 15);
				hash = (hash + 0x0987efab) ^ (hash >> 11);
				hash ^= location.y;
				hash = (hash + 0x46ac12fd) + (hash << 7);
				hash = (hash + 0xbe9730af) ^ (hash << 11);
				Random.InitState((int)hash);

				tileData.sprite = GetSprite();
			}

			tileData.color = this.color;
			tileData.transform = this.transform;
			tileData.gameObject = this.gameObject;
			tileData.flags = this.flags;
			tileData.colliderType = this.colliderType;
		}

		private Sprite GetSprite(){
			//if sprite list is empty, generate it; happens every resize
			if (sprites == null || sprites.Length != totalWeight){
				sprites = new Sprite[totalWeight];

				int n = 0;
				for (int i=0; i<choices.Length; i++){
					for (int j=0; j<choices[i].weight; j++){
						sprites[n++] = choices[i].sprite;
					}
				}
			}
			if (sprites.Length != totalWeight){
				Debug.Log("Error: Random tile not loaded properly. Try re-loading the scene. Actual Total Weight: " + sprites.Length + "; Expected Total Weight: " + totalWeight);
			}
			if (sprites.Length > 0)
				return sprites[Random.Range(0, totalWeight)];
			else return null; //only happens in the same case where the above error message prints. Neither should happen anymore though
		}

		//pair of values: sprite for tile, and weight of tile.
		//weight must be positive. Tiles of weight 0 are never chosen.
		//if all tiles have the same weight, they all have an equal chance of being selected.
		//weight is essentially the number of raffle tickets held by each tile.
		[Serializable]
		public struct SpriteAndWeight{
			public Sprite sprite;
			public int weight;
		}
	}

	#if UNITY_EDITOR
	[CustomEditor(typeof(CDLRandomTile))]
	public class CDLRandomTileEditor : Editor
	{
		private CDLRandomTile tile { get { return (target as CDLRandomTile); } }

		public override void OnInspectorGUI()
		{
			EditorGUI.BeginChangeCheck();
			int count = EditorGUILayout.DelayedIntField("Number of Sprites", tile.choices != null ? tile.choices.Length : 0);
			if (count < 0)
				count = 0;
			if (tile.choices == null || tile.choices.Length != count || tile.dirty)
			{
				tile.dirty = false;
				Array.Resize<CDLRandomTile.SpriteAndWeight>(ref tile.choices, count);
				tile.totalWeight = 0;
				for (int i=0; i<tile.choices.Length; i++){
					tile.totalWeight += tile.choices[i].weight;
				}
			}

			if (count == 0)
				return;

			EditorGUILayout.LabelField("Total weight: " + tile.totalWeight);
			EditorGUILayout.LabelField("Place random sprites.");
			EditorGUILayout.Space();

			for (int i = 0; i < count; i++)
			{
				tile.choices[i].sprite = (Sprite) EditorGUILayout.ObjectField("Sprite " + (i+1), tile.choices[i].sprite, typeof(Sprite), false, null);
				tile.choices[i].weight = (int) EditorGUILayout.IntField("Weight " + (i+1), (tile.choices[i].weight <= 0) ? 1 : tile.choices[i].weight); //prevent zero weight: bad things happen
				EditorGUILayout.LabelField(String.Format("Sprite {0} chance: {1:P}", i+1, (float)tile.choices[i].weight / tile.totalWeight));
				EditorGUILayout.Space();
			}
			if (EditorGUI.EndChangeCheck()){
				tile.dirty = true;
				EditorUtility.SetDirty(tile);
			}
		}
	}
	#endif

}