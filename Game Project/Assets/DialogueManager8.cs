using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManager8 : MonoBehaviour
{

    public Text nameText;
    public Text dialogueText;
    public GameObject chatBox;
    private bool flag = false;
    private int iterator = 0;

    void Start()
    {
        if (GameSaver.liveSave.bossKilled[1] && !flag) DisplayDialogue();
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
            GameSaver.liveSave.tutorialpoint2 = true;
            chatBox.SetActive(true);
            nameText.text = GameSaver.liveSave.playerName;
            dialogueText.text = "(You find yourself back at the teleporter pad, out of breath and shaken.)";
            return;
        }

        if (iterator == 2)
        {
            chatBox.SetActive(false);
            return;
        }


    }
}

