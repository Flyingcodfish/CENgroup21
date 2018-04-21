using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GobbyConversation2 : MonoBehaviour
{

    public GameObject startConvo;
    public GameObject continueConvo;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!GameSaver.liveSave.tutorialpoint3) startConvo.SetActive(true);
            else continueConvo.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            startConvo.SetActive(false);
        }
    }
}
