using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Globalization;

public class DevConsole : MonoBehaviour {

	private enum Command {
		none, echo, load, takeDamage, spendMana
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
					currentMode = Command.load;
					break;
				case "takedamage":
				case "dmg":
					currentMode = Command.takeDamage;
					break;
				case "spendmana":
				case "lmana":
					currentMode = Command.spendMana;
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
			case Command.load:
				DevLoadLevel(word);
				return;
			case Command.takeDamage:
				gameObject.SendMessageUpwards("TakeDamage", Int32.Parse(word, NumberStyles.AllowLeadingSign));
				break;
			case Command.spendMana:
				gameObject.SendMessageUpwards("SpendMana", Int32.Parse(word, NumberStyles.AllowLeadingSign));
				break;
			}
		}
	}

	void DevLoadLevel(string levelName){
		Debug.Log("Loading level: " + levelName +"...");
		SceneManager.LoadScene(levelName);
	}

}
