using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Globalization;

public class DevConsole : MonoBehaviour {

	//only need command modes for multi-word commands
	private enum Command {
		none, echo, loadScene, takeDamage, spendMana, setFlag
	}

	private Command currentMode;

	private PlayerControl player;
	private inventory inv;

	public void TakeCommand(string rawString){
		currentMode = Command.none;
		string cmdString = rawString.ToLower();
		foreach (string word in cmdString.Split(' ')){
			if (currentMode == Command.none){
				switch (word){
				case "echo":
					currentMode = Command.echo;
					break;
				case "load":
				case "loadscene":
					currentMode = Command.loadScene;
					break;
				case "takedamage":
				case "dmg":
					currentMode = Command.takeDamage;
					break;
				case "spendmana":
				case "lmana":
					currentMode = Command.spendMana;
					break;
				case "save":
					GameSaver.SaveGame ();
					return;
				case "name":
					Debug.Log ("Player name is :" + GameSaver.liveSave.playerName);
					return;
				case "freeze":
					WaterTileObject[] water = Resources.FindObjectsOfTypeAll<WaterTileObject> ();
					Debug.Log ("Freezing " + water.Length + " water objects.");
					for (int i = 0; i < water.Length; i++) {
						water [i].Freeze ();
					}
					return;
				case "setflag":
				case "set":
				case "kill": //in the context of killing bosses. If more flags end up being settable with this command it'll make less sense
					currentMode = Command.setFlag;
					break;
				case "loadinv":
				case "linv":
					inv.LoadInventory(string.Empty); // set to empty to load from pref 
					return;
				case "saveinv":
				case "sinv":
					inv.SaveInventory();
					return;
				case "godmode":
				case "god":
					player.isInvincible = !player.isInvincible;
					player.infiniteMana = !player.infiniteMana;
					Debug.Log("Toggling God Mode. Now " + (player.infiniteMana ? "enabled." : "disabled."));
					return;
				case "addkey":
				case "key":
					player.hasKeys ++;
					return;
				default:
					Debug.Log("Error: '" + word + "' is not a recognzed command.");
					return;
				}
				continue;
			}

			//we've received a multi-word command, time to execute based on the following word
			switch (currentMode) {
			case Command.echo:
				Debug.Log (word);
				return;
			case Command.loadScene:
				DevLoadLevel (word);
				return;
			case Command.takeDamage:
				gameObject.SendMessageUpwards ("TakeDamage", Int32.Parse (word, NumberStyles.AllowLeadingSign));
				break;
			case Command.spendMana:
				gameObject.SendMessageUpwards ("SpendMana", Int32.Parse (word, NumberStyles.AllowLeadingSign));
				break;
			case Command.setFlag:
				switch (word) {
				case "boss0":
				case "fire_boss":
					GameSaver.liveSave.bossKilled [0] = true;
					break;
				case "boss1":
				case "water_boss":
					GameSaver.liveSave.bossKilled [1] = true;
					break;
				case "boss2":
				case "maze_boss":
				case "final_boss":
					GameSaver.liveSave.bossKilled [2] = true;
					break;
				case "watertut":
					GameSaver.liveSave.watertutorialpoint = true;
					break;
				case "firetut":
					GameSaver.liveSave.firetutorialpoint = true;
					break;
				case "mazetut":
					GameSaver.liveSave.mazetutorialpoint = true;
					break;
				}
				break; //break setflag case
			default:
				Debug.Log ("The dev console reached a stage it shouldn't. pls to fix");
				break;
			}
		}
	}

	void DevLoadLevel(string levelName){
		Debug.Log("Loading level: " + levelName +"...");
		SceneManager.LoadScene(levelName);
	}

	void Start(){
		player = GetComponentInParent<PlayerControl>();
		inv = player.inventory;
	}

}
