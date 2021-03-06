﻿using System.Collections;
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
		GameObject.FindObjectOfType<PlayerControl> ().lockAI -= 1;
        nameText.text = name;
		GameSaver.liveSave.playerName = name;
		GameSaver.liveSave.hasBeenNamed = true;
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
