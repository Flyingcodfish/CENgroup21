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
    public GameObject arrow1;
    public GameObject exclamation;
    public GameObject exclamation2;
    public GameObject question;
    public GameObject question2;
    public GameObject firearrow;
    public GameObject firearrow2;
    public GameObject waterarrow;
    public GameObject waterarrow2;
    public GameObject mazearrow;
    public GameObject mazearrow2;
    private int iterator = 0;

    void Start()
    {
        if (!GameSaver.liveSave.firetutorialpoint) DisplayDialogue();
        else
        {
            arrow1.SetActive(false);
            exclamation.SetActive(false);
            exclamation2.SetActive(false);
            question.SetActive(false);
            question2.SetActive(false);
            if (!GameSaver.liveSave.watertutorialpoint)
            {
                firearrow.SetActive(true);
                firearrow2.SetActive(true);
            }
            else if (GameSaver.liveSave.watertutorialpoint && !GameSaver.liveSave.mazetutorialpoint)
            {
                waterarrow.SetActive(true);
                waterarrow2.SetActive(true);
            }
            else if (GameSaver.liveSave.mazetutorialpoint)
            {
                mazearrow.SetActive(true);
                mazearrow2.SetActive(true);
            }
        }
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
            nameText.text = GameSaver.liveSave.playerName;
            dialogueText.text = "(You find yourself in a quaint town. Villagers seem to perk up at the news of your arrival, and you’re met with hopeful smiles from unfamiliar people.)";
            return;
        }

        if (iterator == 1)
        {
            nameText.text = "Gobby";
            dialogueText.text = GameSaver.liveSave.playerName + ", over here!";
            return;
        }

        if (iterator == 2)
        {
            chatBox.SetActive(false);
        }
    }
}

