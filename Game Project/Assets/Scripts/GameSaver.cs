using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

//not static, but there should only be one.
//It's a way of implenting a singleton class. Basically static, but it can store instances (like liveSave).
public static class GameSaver {

	//this is a global field: GameSaver.liveSave can be referenced anywhere to access/change the current game state.
	public static SavedGame liveSave = new SavedGame(); //the "live" version (in RAM) of the most recently loaded or created save. Written to disk when game is saved, changed when game is loaded.

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
			liveSave.coins = player.coins;

			//save inventory data
			player.inventory.SaveInventory();
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
//stores data of a saved game. Instantiated and filled with data on load.
[System.Serializable]
public class SavedGame {

	//basic save data
	public string playerName = "(name not assigned)";
//	public int slot; //only used if we want to have multiple save slots, requires more work and an extra main menu screen
	public bool hasBeenSaved = false; //used by playerControl to determine if it should load traits from this saved file (true) or use its own default values (false).
	public bool hasBeenNamed = false;


	//PlayerControl fields
	public float currentMana;
	public int currentHealth;
	public int hasKeys;
	public int coins;
	public string sceneName; //scene player last entered; we might want more granular control with multiple spawnPoints per scene
//	public string location; //more specific location; for when there are multiple spawnpoints in a scene. Not currently implemented
	//TODO: up player power, strength, speed

	//player inventory
	public SavedInventory playerInventoryData = new SavedInventory();


	//unlocks and mechanical progression
	public bool[] bossKilled = new bool[3]; //array of three bools, indexed by boss number. Default values are false
	public bool[] padUnlocked = new bool[3]; //array of three bools, indexed by teleport pad number. Default values are false
	public bool[] spellTaken = new bool[3]; //0 - ice spell; 1 - fireball; 2 - push spell. False: not yet picked up, item should spawn. True: item claimed, should not be present on ground.


	//used to make doors stay unlocked, and keys stay picked up when a game is loaded. Currently only supports having up to 4,294,967,296 unique doors or keys.
	public List<int> unlockedDoors = new List<int>(); //dictionary of which doors have been unlocked. If a door is unlocked, it will generate a unique int hash and add itself to this dictionary, with a value of true.
	public List<int> pickedUpKeys = new List<int>(); //dictionary of which keys have been picked up. If a key is picked up, it will generate a unique int hash and add itself to this dictionary, with a value of true.

	//TODO: set of flags indicating which upgrades have been unlocked.


	//story progression flags
	public bool firespell = false;
	public bool firetutorialpoint = false;
	public bool watertutorialpoint = false;
    public bool mazetutorialpoint = false;

}

[System.Serializable]
public class SavedInventory {
	public string content = "";
	public int slots = 10;
	public int rows = 1;
	public float slotPaddingLeft = 2f;
	public float slotPaddingTop = 2f;
	public float slotSize = 30f;
}
