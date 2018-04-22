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
    public GameObject Lato;
    public GameObject Gobby;
    public GameObject Gobby2;
    public GameObject Gobby3;
    private int iterator = 0;

    void Start()
    {
        iterator = 0;
        if (GameSaver.liveSave.bossKilled[0] && GameSaver.liveSave.tutorialpoint)
        {
            Gobby.SetActive(false);
            arrow1.SetActive(true);
        }
        else if (GameSaver.liveSave.tutorialpoint4 && !GameSaver.liveSave.tutorialpoint2)
        {
            arrow1.SetActive(false);
            Gobby.SetActive(false);
            Gobby2.SetActive(true);
        }
        else if (GameSaver.liveSave.tutorialpoint2)
        {
            waterarrow.SetActive(true);
            Lato.SetActive(true);
            Gobby3.SetActive(true);
        }
        if (!GameSaver.liveSave.firetutorialpoint) DisplayDialogue();
        else
        {
            arrow1.SetActive(false);
            exclamation.SetActive(false);
            exclamation2.SetActive(false);
            question.SetActive(false);
            question2.SetActive(false);
            if (!GameSaver.liveSave.bossKilled[0])
            {
                firearrow.SetActive(true);
                firearrow2.SetActive(true);
            }
        }
    }
    public void clearIterator()
    {
        iterator = 0;
        DisplayDialogue4();
    }

    public void iterate()
    {
        iterator++;
        if (!GameSaver.liveSave.tutorialpoint) DisplayDialogue();
        else if (GameSaver.liveSave.tutorialpoint4 && !GameSaver.liveSave.tutorialpoint2) DisplayDialogue2();
        else if (GameSaver.liveSave.tutorialpoint2 && !GameSaver.liveSave.tutorialpoint3) DisplayDialogue3();
        else if (GameSaver.liveSave.tutorialpoint3) DisplayDialogue4();
    }

    public void DisplayDialogue4()
    {
        if (iterator == 0)
        {
            nameText.text = GameSaver.liveSave.playerName;
            dialogueText.text =  "(When you speak to them again, they don’t realize you’re there.)";
            return;
        }

        if (iterator == 1)
        {
            nameText.text = "Gobby";
            dialogueText.text = "But should we ask them? Maybe we’ll get through to them.";
            return;
        }

        if (iterator == 2)
        {
            nameText.text = "Lato";
            dialogueText.text = "And for what? They won’t remember.";
            return;
        }

        if (iterator == 3)
        {
            nameText.text = "Gobby";
            dialogueText.text = "Maybe we can get through to them. Maybe some part of them will be able to give us an answer.";
            return;
        }

        if (iterator == 4)
        {
            nameText.text = GameSaver.liveSave.playerName;
            dialogueText.text = "(Both of them realize you’re there.)";
            return;
        }

        if (iterator == 5)
        {
            nameText.text = "Gobby";
            dialogueText.text = GameSaver.liveSave.playerName + "! I didn’t realize you were back. If you think you’re ready to final trial, you can step through whenever you’d please.";
            return;
        }

        if (iterator == 6)
        {
            nameText.text = "";
            GameSaver.liveSave.mazetutorialpoint = true;
            dialogueText.text = "You can now access the Maze Dungeon by stepping on the ominous teleport to the north.";
            return;
        }

        if (iterator == 6)
        {
            chatBox.SetActive(false);
        }
    }

    public void DisplayDialogue3()
    {
        if (iterator == 0)
        {
            waterarrow.SetActive(false);
            nameText.text = "Gobby";
            dialogueText.text = GameSaver.liveSave.playerName + "! You’ve already finished the second trial! I can see the corruption starting to wane.";
            return;
        }

        if (iterator == 1)
        {
            nameText.text = "Lato";
            dialogueText.text = "Indeed, but the worst is yet to come. I recommend you make sure you’re well prepared for the final challenge before returning.";
            return;
        }

        if (iterator == 2)
        {
            nameText.text = "";
            dialogueText.text = "(You may leave, go to the market, or continue.)";
            return;
        }

        if (iterator == 3)
        {
            chatBox.SetActive(false);
            GameSaver.liveSave.tutorialpoint3 = true;
        }
    }

        public void DisplayDialogue2()
    {
        if (iterator == 0)
        {
            nameText.text = "Gobby";
            dialogueText.text = "Hi, " + GameSaver.liveSave.playerName + "! Since you’re back, I assume you finished your first trial. Congratulations!";
            return;
        }

        if (iterator == 1)
        {
            dialogueText.text = "I know you forgot most things, so I wanted to re-introduce you to the merchant. They have plenty of things you might find useful as you complete your trials again.";
            return;
        }

        if (iterator == 2)
        {
            nameText.text = "";
            dialogueText.text = "Press 'E' to use the shop, then click 'Continue.'";
            return;
        }

        if (iterator == 3)
        {
            nameText.text = "Gobby";
            dialogueText.text = "I guess you should be on your way now! Thank you for doing this, " + GameSaver.liveSave.playerName + ".";
            return;
        }

        if (iterator == 4)
        {
            dialogueText.text = "I know you have to in order to get your spells back, but I can’t emphasize just how much you’re doing for us.";
            return;
        }

        if (iterator == 5)
        {
            dialogueText.text = "Things haven’t been too good lately with the corruption closing in on us. Hopefully with your help, we can get some normalcy again.";
            return;
        }

        if (iterator == 6)
        {
            nameText.text = "";
            dialogueText.text = "You can now access the Water Dungeon by stepping on the blue teleport to the west.";
            GameSaver.liveSave.watertutorialpoint = true;
            mazearrow.SetActive(true);
            mazearrow2.SetActive(true);
            return;
        }

        if (iterator == 7)
        {
            chatBox.SetActive(false);
        }
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

