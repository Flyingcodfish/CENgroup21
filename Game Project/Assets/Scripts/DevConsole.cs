using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Globalization;

public class DevConsole : MonoBehaviour {

	private enum Command {
		none, echo, loadScene, takeDamage, spendMana, save
	}

	private Command currentMode;

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
					currentMode = Command.save;
					break;
				case "name":
					Debug.Log ("Player name is :" + GameSaver.liveSave.playerName);
					break;
				default:
					Debug.Log("Error: '" + word + "' is not a recognzed command.");
					return;
				}
				continue;
			}

			//we've parsed a command, time to execute it
			switch (currentMode){
			case Command.echo:
				Debug.Log(word);
				return;
			case Command.loadScene:
				DevLoadLevel(word);
				return;
			case Command.takeDamage:
				gameObject.SendMessageUpwards("TakeDamage", Int32.Parse(word, NumberStyles.AllowLeadingSign));
				break;
			case Command.spendMana:
				gameObject.SendMessageUpwards("SpendMana", Int32.Parse(word, NumberStyles.AllowLeadingSign));
				break;
			case Command.save:
				GameSaver.SaveGame ();
				break;
			}
		}
	}

	void DevLoadLevel(string levelName){
		Debug.Log("Loading level: " + levelName +"...");
		SceneManager.LoadScene(levelName);
	}

}
