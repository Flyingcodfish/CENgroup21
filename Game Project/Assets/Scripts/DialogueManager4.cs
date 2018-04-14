using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManager4 : MonoBehaviour
{

    public Text nameText;
    public Text dialogueText;
    public GameObject chatBox;
    public GameObject exclamation;
    public GameObject exclamation2;
    public GameObject elipses;
    public GameObject continueButton;
    public GameObject test;
    private int iterator = 0;

    void Start()
    {
        DisplayDialogue();
    }

    public void iterate()
    {
        if (iterator != 9) iterator++;
        DisplayDialogue();
    }

    public void DisplayDialogue()
    {
        if (iterator == 0)
        {
            chatBox.SetActive(true);
            nameText.text = GameSaver.gameSaverInstance.liveSave.playerName;
            dialogueText.text = "(Gobby takes you into a house, where another unfamiliar face greets you with familiarity.)";
            return;
        }

        if (iterator == 1)
        {
            nameText.text = "Gobby";
            dialogueText.text = "Lato! You’ll never guess what I fished up this time!";
            return;
        }

        if (iterator == 2)
        {
            nameText.text = "Lato";
            dialogueText.text = "If it’s another bass, I’ll tell you again: you’re not gonna top my Catch of the Year.";
            return;
        }

        if (iterator == 3)
        {
            exclamation.SetActive(true);
            dialogueText.text = "Gobby and " + GameSaver.gameSaverInstance.liveSave.playerName + "! That’s a duo I wasn’t expecting to see again.";
            return;
        }

        if (iterator == 4)
        {
            exclamation.SetActive(false);
            nameText.text = "Gobby";
            dialogueText.text = "I found this one washed up on the shore. They’re acting a bit strange, though.";
            return;
        }

        if (iterator == 5)
        {
            elipses.SetActive(true);
            nameText.text = "Lato";
            dialogueText.text = "Huh... This is actually familiar to me.";
            return;
        }

        if (iterator == 6)
        {
            elipses.SetActive(false);
            nameText.text = GameSaver.gameSaverInstance.liveSave.playerName;
            dialogueText.text = "(Lato steps closer and examines you.)";
            return;
        }

        if (iterator == 7)
        {
            nameText.text = "Lato";
            dialogueText.text = GameSaver.gameSaverInstance.liveSave.playerName + ", mind lighting a fire for me?";
            return;
        }

        if (iterator == 8)
        {
            nameText.text = "";
            dialogueText.text = "Press the number that corresponds to the position of the fire spellbook in your inventory, then click 'continue.'";
        }

        if (iterator == 9)
        {
            if (!GameSaver.gameSaverInstance.liveSave.firespell) DisplayDialogue();
            else
            {
                iterator++;
                DisplayDialogue();
            }
        }

        if (iterator == 10)
        {
            nameText.text = "Lato";
            dialogueText.text = "Weak! Thats what I thought. It’s common among certain sorcerers to come down with amnesia if they work with dangerous catalysts.";
            return;
        }

        if (iterator == 11)
        {
            nameText.text = "Gobby";
            dialogueText.text = "So you’re saying " + GameSaver.gameSaverInstance.liveSave.playerName + " doesn’t remember me? Or you?";
            return;
        }

        if (iterator == 12)
        {
            nameText.text = "Lato";
            dialogueText.text = "Or anybody here, on that note. Or any magic.";
            return;
        }

        if (iterator == 13)
        {
            nameText.text = "Gobby";
            dialogueText.text = "(sighs) Right when we needed you most. Have we been cursed?";
            return;
        }

        if (iterator == 14)
        {
            nameText.text = "Lato";
            dialogueText.text = "Perhaps. But we have the state sorcerer here, nevertheless. Perhaps by helping them regain their memory, we can save ourselves as well.";
            return;
        }

        if (iterator == 15)
        {
            nameText.text = GameSaver.gameSaverInstance.liveSave.playerName;
            dialogueText.text = "M-me..?";
            return;
        }

        if (iterator == 16)
        {
            nameText.text = "Lato";
            dialogueText.text = "You probably don’t remember, but you were a very powerful sorcerer before losing your memory. While you may have forgotten your spells, the fact remains that you are a capable sorcerer.";
            return;
        }

        if (iterator == 17)
        {
            nameText.text = "Gobby";
            dialogueText.text = "Does that mean they can get their spells back?";
            return;
        }

        if (iterator == 18)
        {
            nameText.text = "Lato";
            dialogueText.text = "Thats exactly what it means. It’s here that you gained your abilities, and it’s here that you’ll gain them back. You became state sorcerer after all.";
            return;
        }

        if (iterator == 19)
        {
            exclamation2.SetActive(true);
            nameText.text = "Gobby";
            dialogueText.text = "But how can that be done?";
            return;
        }

        if (iterator == 20)
        {
            exclamation2.SetActive(false);
            nameText.text = "Lato";
            dialogueText.text = "They’ll have to challenge the temples again, alone.";
            return;
        }

        if (iterator == 21)
        {
            nameText.text = "Gobby";
            dialogueText.text = "But the temples are overrun with corruption...";
            return;
        }

        if (iterator == 22)
        {
            nameText.text = "Lato";
            dialogueText.text = "I know, it’ll be more difficult than before. But they’re more powerful than before, despite not having the abilities to prove it.";
            return;
        }

        if (iterator == 23)
        {
            nameText.text = "Gobby";
            dialogueText.text = GameSaver.gameSaverInstance.liveSave.playerName + ", is this okay with you? You don’t have to do this.";
            return;
        }

        if (iterator == 24)
        {
            nameText.text = GameSaver.gameSaverInstance.liveSave.playerName;
            dialogueText.text = "I guess..?";
            return;
        }

        if (iterator == 25)
        {
            nameText.text = "Gobby";
            dialogueText.text = "Well... okay! I know you’re capable of doing this. If you go to the Fire Dungeon, you’ll learn to cast Fireball. That’s how you’ll start your journey.";
            return;
        }

        if (iterator == 26)
        {
            nameText.text = "";
            dialogueText.text = "You can now access the Fire Dungeon by stepping on the orange teleport pad at the beach";
            GameSaver.gameSaverInstance.liveSave.firetutorialpoint = true;
            return;
        }

        if (iterator == 27)
        {
            nameText.text = "Gobby";
            dialogueText.text = GameSaver.gameSaverInstance.liveSave.playerName + ", let's go back to the beach! Bye, Lato!";
            return;
        }

        if (iterator == 28)
        {
            nameText.text = "Lato";
            dialogueText.text = "(waves) Take care, Gobby! And best of luck to you, Sorcerer " + GameSaver.gameSaverInstance.liveSave.playerName + ".";
            return;
        }

        if (iterator == 29)
        {
            SceneManager.LoadScene("test");
        }
    }
}

