using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GobbyConversation : MonoBehaviour {

    public GameObject startConvo;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!GameSaver.gameSaverInstance.liveSave.firetutorialpoint) startConvo.SetActive(true);
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
