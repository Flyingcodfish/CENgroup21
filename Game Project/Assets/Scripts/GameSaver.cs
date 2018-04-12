using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//not static, but there should only be one.
//It's a way of implenting a singleton class. Basically static, but it can store instances (like liveSave).
public class GameSaver {

	public static GameSaver gameSaverInstance; 	//global field: rather than using FindObjectWithTag or something,
												//a permanent reference to the singleton GameSaver instance is at "GameSaver.gameSaverInstance"

	public SavedGame liveSave; //the "live" version (in RAM) of the most recently loaded or created save. Written to disk when game is saved, changed when game is loaded.


	public GameSaver(){
		if (gameSaverInstance == null) {
			gameSaverInstance = this;
			liveSave = new SavedGame ();
		}
		else {
			Debug.Log ("Error: there should never be more than one gameSaver. Deleting the newer instance.");
		}
	}

	public bool SaveGame(/*int saveSlot*/) {
		//TODO: collect data from live save passed in. encrypt data and write it to a file
		return true;
	}


	public bool LoadGame(/*int saveSlot*/) {
		//TODO: read and decrypt saved game data from file; write it to the passed SavedGame object reference
		//TODO: add code to playerControl script that sets all relevant PlayerControl fields
		return true; //returns true if successful
	}


	//	public static SaveList GetSaveList (){
	//		//TODO: returns a yet undefined struct of save slots that exist. Intended to be called from a potential "choose a save slot" screen.
	//		//Grabs basic information like slot number, name and inventory from each existing save, so that one can be chosen and loaded with the slot number.
	//	}

}

public class SavedGame {
	//stores data of a saved game. Instantiated and filled with data on load.
	public string playerName = "(name not assigned)";
	public int slot; //only used if we want to have multiple save slots, requires more work and an extra main menu screen
	public string[] playerInventory; //TODO: integrate with Tyler's existing inventory saving/loading (rather than using playerPrefs). Can be something other than a string array

	//fields we'll want but aren't yet functional
	public float mana;
	public float currentHealth;
	public string location; //scene player last entered; we might want more granular control with multiple spawnPoints per scene

	public bool[] bossKilled; //array of three bools, indexed by boss
	public bool[] padUnlocked; //array of three bools, indexed by teleoport pad

	//TODO: add even more bools that represent progress in dialogues, in case we want some things to not be repeated ever
	//to make characters say different thigns as the game progresses, we can either use the above boss-killed flags to determine what a character says,
	//or determine what is said using flags that are declared here (for more granular control)
}
