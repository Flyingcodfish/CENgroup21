using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

/*
 * ==== DEATH DROPPER ====
 * This component can be added to a GameObject or any one of its children.
 * When this object's Drop() method is called, an item randomly chosen from
 * its predefined drop list will be instantiated, based on the weights of each drop option.
 * This can be done with a direct call to the DeathDropper component, or via a message.
 *
 * There is a DefaultDeathDropper prefab in "Assets/Prefabs/Drops" that can be added
 * as a child object to any existing GameObject. This is merely a matter of standardization
 * and convenience: This prefab has a DeathDropper component with values that most enemies will probably want to use.
 * If one wants a specific enemy to have altered drop chances or unique drops (such as keys),
 * this can easily be done by adding the DeathDropper component to the Actor in question, and modifying it
 * in the inspector.
 * 
 * Special note: The Actor class broadcasts a "Drop" method to itself and all its children
 * when it dies. This means that any Actor that has a DeathDropper component, or a child
 * object with the DeathDropper component, will drop things on death automatically. No additional scripting
 * is required in most cases.
 * 
 */

public class DeathDropper : MonoBehaviour {

	public DropListEntry[] dropList;
	public int totalWeight;
	public bool dirty = false;

	public void Drop(){
		int choice = UnityEngine.Random.Range (0, totalWeight);
		int dropIndex = 0;
		int currentWeight = 0;
		while (choice > currentWeight + dropList [dropIndex].weight) {
			currentWeight += dropList [dropIndex].weight;
			dropIndex++;
		}
		GameObject chosenDrop = dropList [dropIndex].drop;
		if (chosenDrop != null) Instantiate (chosenDrop, transform.position, Quaternion.identity);
	}



	//struct for drop list
	[Serializable]
	public struct DropListEntry{
		public int weight;
		public GameObject drop;
	}
	
	//custom editor for assigning values in the inspector
	#if UNITY_EDITOR
	[CustomEditor(typeof(DeathDropper))]
	public class DeathDropperEditor : Editor
	{
		private DeathDropper dropper { get { return (target as DeathDropper); } }

		public override void OnInspectorGUI()
		{
			EditorGUI.BeginChangeCheck();
			int count = EditorGUILayout.DelayedIntField("Number of Options", dropper.dropList != null ? dropper.dropList.Length : 0);
			if (count < 0)
				count = 0;
			if (dropper.dropList == null || dropper.dropList.Length != count || dropper.dirty)
			{
				dropper.dirty = false;
				Array.Resize<DeathDropper.DropListEntry>(ref dropper.dropList, count);
				dropper.totalWeight = 0;
				for (int i = 0; i < dropper.dropList.Length; i++) {
					dropper.totalWeight += dropper.dropList [i].weight;
				}
			}

			if (count == 0)
				return;

			EditorGUILayout.LabelField("Total weight: " + dropper.totalWeight);
			EditorGUILayout.LabelField("Assign all possible drops, including a null drop, if desired.");
			EditorGUILayout.Space();

			for (int i = 0; i < count; i++)
			{
				dropper.dropList[i].drop = (GameObject) EditorGUILayout.ObjectField("Drop " + (i+1), dropper.dropList[i].drop, typeof(GameObject), false, null);
				dropper.dropList[i].weight = (int) EditorGUILayout.IntField("Weight " + (i+1), (dropper.dropList[i].weight <= 0) ? 1 : dropper.dropList[i].weight); //prevent zero weight: bad things happen
				EditorGUILayout.LabelField(String.Format("Drop {0} chance: {1:P}", i+1, (float)dropper.dropList[i].weight / dropper.totalWeight));
				EditorGUILayout.Space();
			}
			if (EditorGUI.EndChangeCheck()){
				dropper.dirty = true;
				EditorUtility.SetDirty(dropper);
			}
		}
	}
	#endif
}