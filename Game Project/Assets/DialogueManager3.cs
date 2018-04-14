using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManager3 : MonoBehaviour
{

    public Text nameText;
    public Text dialogueText;
    public GameObject chatBox;
    private int iterator = 0;

    void Start()
    {
        DisplayDialogue();
    }

    public void iterate()
    {
        iterator++;
        DisplayDialogue();
    }

    public void DisplayDialogue()
    {
        if (iterator == 0)
        {
            chatBox.SetActive(true);
            nameText.text = GameSaver.gameSaverInstance.liveSave.playerName;
            dialogueText.text = "(You find yourself in a quaint town. Villagers seem to perk up at the news of your arrival, and you’re met with hopeful smiles from unfamiliar people.)";
            return;
        }

        if (iterator == 1)
        {
            nameText.text = "Gobby";
            dialogueText.text = GameSaver.gameSaverInstance.liveSave.playerName + ", over here!";
            return;
        }

        if (iterator == 2)
        {
            chatBox.SetActive(false);
        }
    }
}

