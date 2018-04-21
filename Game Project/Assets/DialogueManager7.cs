using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManager7 : MonoBehaviour
{
   
    public Text nameText;
    public Text dialogueText;
    public GameObject chatBox;
    private bool flag = false;
    private int iterator = 0;

    void Update()
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
            chatBox.SetActive(true);
            nameText.text = GameSaver.liveSave.playerName;
            dialogueText.text = "(You remember secrets. You remember power. You remember wanting more power.)";
            return;
        }

        if (iterator == 1)
        {
            dialogueText.text = "(You remember wanting to do anything for it.)";
            return;
        }

        if (iterator == 2)
        {
            dialogueText.text = "(You remember doing everything for it. You remember speaking in forbidden tongues and reading forbidden books.)";
            return;
        }

        if (iterator == 3)
        {
            dialogueText.text = "(You remember warnings of consequences. You remember the pain. You remember how you’d do anything for it. You remember how badly you wanted it.)";
            return;
        }

        if (iterator == 4)
        {
            dialogueText.text = "(Why did you do that?)";
            return;
        }

        if (iterator == 5)
        {
            SceneManager.LoadScene("dungeon1_entrance");
        }
    }
}

