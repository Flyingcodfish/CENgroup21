using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

//not static, but there should only be one.
//It's a way of implenting a singleton class. Basically static, but it can store instances (like liveSave).
public static class GameSaver {

	//this is a global field: GameSaver.liveSave can be reference anywhere to access/change the current game state.
	public static SavedGame liveSave; //the "live" version (in RAM) of the most recently loaded or created save. Written to disk when game is saved, changed when game is loaded.

	public static bool SaveGame(/*int saveSlot*/) {
		//tell the save file that it's been saved. If it's loaded later, PlayerControl will see this fact and load traits from the live save.
		liveSave.hasBeenSaved = true;
		//save the current scene name. Should we save actual x,y location? probably not.
		liveSave.sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene ().name;

		//write PlayerControl fields to liveSave, and stage for writing to disk.
		PlayerControl player = Object.FindObjectOfType<PlayerControl>();
		Debug.Log ("Saving. Player found? " + ((player == null) ? "N" : "Y"));
		if (player != null){
			liveSave.currentMana = player.currentMana;
			liveSave.currentHealth = player.currentHealth;
			liveSave.hasKeys = player.hasKeys;
			liveSave.hasMoney = player.hasMoney; //TODO: integrate with tyler's stuff. is this the correct field name?
		}

		//serialize data from live save. write to a file.
		BinaryFormatter serializer = new BinaryFormatter();
		Directory.CreateDirectory (Application.dataPath + "/Saves/"); //creates saves directory if it's not already present
		FileStream saveFile = File.Create (Application.dataPath + "/Saves/save.mg21");
		serializer.Serialize (saveFile, liveSave);
		saveFile.Close ();
		Debug.Log ("Saved game to " + Application.dataPath + "/Saves/save.mg21");
		return true;
	}


	public static bool LoadGame(/*int saveSlot*/) {
		//read and deserialize saved game data from file
		BinaryFormatter deserializer = new BinaryFormatter();
		FileStream saveFile = File.OpenRead (Application.dataPath + "/Saves/save.mg21");
		liveSave = (SavedGame)deserializer.Deserialize (saveFile);
		saveFile.Close ();
		return true; //returns true if successful
	}


	//	public static SaveList GetSaveList (){
	//		//TODO: returns a yet undefined struct of save slots that exist. Intended to be called from a potential "choose a save slot" screen.
	//		//Grabs basic information like slot number, name and inventory from each existing save, so that one can be chosen and loaded with the slot number.
	//	}

}

//serializable: can be converted into straight binary data, which we will read from / write to a file on disk
[System.Serializable]
public class SavedGame {
	//stores data of a saved game. Instantiated and filled with data on load.
	public string playerName = "(name not assigned)";
	public bool hasBeenSaved = false; //used by playerControl to determine if it should load traits from this saved file (true) or use its own default values (false).

	//public int saveSlot; //only used if we want to have multiple save slots, requires more work and an extra main menu screen
	public string[] playerInventory; //TODO: integrate with Tyler's existing inventory saving/loading (rather than using playerPrefs). Can of course be something other than a string array.

	//PlayerControl fields
	public float currentMana;
	public int currentHealth;
	public int hasKeys;
	public int hasMoney; //TODO: integrate with tyler's stuff. is this the correct field name?
	public string sceneName; //scene player last entered; we might want more granular control with multiple spawnPoints per scene
//	public string location; //more specific location; for when there are multiple spawnpoints in a scene. Not currently implemented

	//story progression fields
	public bool[] bossKilled = new bool[3]; //array of three bools, indexed by boss number. Default values are false
	public bool[] padUnlocked = new bool[3]; //array of three bools, indexed by teleport pad number. Default values are false

	//if upgrades are implemented, we can have a set of flags indicating which have been unlocked

	//TODO: add even more bools that represent progress in dialogues, in case we want some things to not be repeated ever
	//to make characters say different thigns as the game progresses, we can either use the above boss-killed flags to determine what a character says,
	//or determine what is said using flags that are declared here (for more granular control)


	//TODO: give all keys and spell items fields here so that the player cannot pick up a key/spell, save/load, and have it respawn.
	//TODO: give all locked door fields here so that an unlocked door will stay unlocked
}
