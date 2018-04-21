using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManager5 : MonoBehaviour
{

    public Text nameText;
    public Text dialogueText;
    public GameObject chatBox;
    private bool flag = false;
    private int iterator = 0;

    void Update()
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
            chatBox.SetActive(true);
            nameText.text = GameSaver.liveSave.playerName;
            dialogueText.text = "(Your head grows heavy again, the way it did at the beach.)";
            return;
        }

        if (iterator == 1)
        {
            dialogueText.text = "(You remember... something.)";
            return;
        }

        if (iterator == 2)
        {
            dialogueText.text = "(You remember secrets. You remember things you couldn’t tell anyone.)";
            return;
        }

        if (iterator == 3)
        {
            dialogueText.text = "(You remember distancing yourself. You remember losing people you’d call family.)";
            return;
        }

        if (iterator == 4)
        {
            dialogueText.text = "(You feel your heart breaking and you don’t know why.)";
            return;
        }

        if (iterator == 5)
        {
            dialogueText.text = "(Why did you leave them?)";
            return;
        }

        if (iterator == 6)
        {
            dialogueText.text = "(Why did you hurt them?)";
            return;
        }

        if (iterator == 7)
        {
            dialogueText.text = "(Why can’t you remember?)";
            return;
        }

        if (iterator == 8)
        {
			chatBox.SetActive (false);
        }
    }
}

