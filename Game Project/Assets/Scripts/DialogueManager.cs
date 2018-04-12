using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    public Text nameText;
    public Text dialogueText;
    public GameObject chatBox;
    public GameObject arrow1;
    public GameObject arrow2;
    [SerializeField]
    private InputField input;

    private Queue<string> sentences;

	// Use this for initialization
	void Start () {
        sentences = new Queue<string>();
	}

    public void GetInput(string name)
    {
        nameText.text = name;
		//this sets the "live" save's player name to be whatever we just retreived.
		//can be referenced from anywhere, in exactly the way it was assigned to here.
		//the fields available in liveSave can be seen/edited in GameSaver.cs, at the bottom.
		//There is a class SavedGame, with all save-able fields, of which liveSave is an instance.
		GameSaver.gameSaverInstance.liveSave.playerName = name;
    }

    public void StartDialogue(Dialogue dialogue)
    {
        if (dialogue.name != "")
        {
            nameText.text = dialogue.name;
        }
        sentences.Clear();

        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }
	
    void EndDialogue()
    {
        chatBox.SetActive(false);
        arrow1.SetActive(true);
        arrow2.SetActive(true);
    }
	
}
