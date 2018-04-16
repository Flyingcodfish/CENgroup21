using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManager2 : MonoBehaviour
{

    public Text nameText;
    public string playerName;
    public Text dialogueText;
    public GameObject chatBox;
    public GameObject IntroBox;
    public GameObject arrow1, arrow2, arrow3, arrow4;
    public GameObject exclamation;
    public GameObject items;
    private int iterator = 0;
    [SerializeField]
    private InputField input;

    public void GetInput(string name)
    {
        playerName = name;
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
            if (IntroBox.activeSelf) IntroBox.SetActive(false);
            arrow1.SetActive(false);
            arrow2.SetActive(false);
            nameText.text = "Gobby";
            dialogueText.text = playerName + "! What are you doing here? Weren’t you supposed to be back in the mainland?";
            return;
        }

        if (iterator == 1)
        {
            nameText.text = playerName;
            dialogueText.text = "(" + playerName + "... I guess that's me.)";
            return;
        }

        if (iterator == 2)
        {
            dialogueText.text = "(Yes, that sounds about right.)";
            return;
        }

        if (iterator == 3)
        {
            dialogueText.text = "(But I have no idea who this person is.)";
            return;
        }

        if (iterator == 4)
        {
            nameText.text = "Gobby";
            dialogueText.text = playerName + ", is everything okay? You seem a little... distant.";
            return;
        }

        if (iterator == 5)
        {
            nameText.text = playerName;
            dialogueText.text = "(You nod hesitantly.)";
            return;
        }

        if (iterator == 6)
        {
            nameText.text = "Gobby";
            dialogueText.text = "Hm. If you say so. Not that I’m doubting you, but I think it’d be a good idea for you to come back to town with me. The island hasn’t really been safe these days.";
            return;
        }

        if (iterator == 7)
        {
            exclamation.SetActive(true);
            dialogueText.text = "Sorry! I know you’re a state sorcerer now, and you can probably take care of yourself. But... strange things have been happening. The temples aren’t even safe anymore.";
            return;
        }

        if (iterator == 8)
        {
            exclamation.SetActive(false);
            if (items.transform.childCount == 0)
            {
                dialogueText.text = "Anyways, follow me.";
                return;
            }
            else
            {
                arrow3.SetActive(true);
                arrow4.SetActive(true);
                dialogueText.text = "Anyways, go collect all those potions and spellbooks over there then follow me! They're going to come in handy.";
                return;
            }
        }
        
        if (iterator == 9)
        {
            if (items.transform.childCount == 0)
            {
                arrow3.SetActive(false);
                arrow4.SetActive(false);
                chatBox.SetActive(false);
                SceneManager.LoadScene("hub");
            }
            else DisplayDialogue();
        }
    }
}

