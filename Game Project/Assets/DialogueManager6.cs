using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManager6 : MonoBehaviour
{

    public Text nameText;
    public Text dialogueText;
    public GameObject chatBox;
    public GameObject arrowBack;
    private bool flag = false;
    private int iterator = 0;

    void Start()
    {
        if (GameSaver.liveSave.bossKilled[0] && !flag) DisplayDialogue();
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
            flag = true;
            GameSaver.liveSave.tutorialpoint = true;
            chatBox.SetActive(true);
            nameText.text = GameSaver.liveSave.playerName;
            dialogueText.text = "(You find yourself back at the teleporter pad, contemplating what you left behind.)";
            return;
        }

        if (iterator == 1)
        {
            dialogueText.text = "Maybe I should go back and talk to Lato...";
            return;
        }

        if (iterator == 2)
        {
            chatBox.SetActive(false);
            arrowBack.SetActive(true);
            return;
        }

        
    }
}

