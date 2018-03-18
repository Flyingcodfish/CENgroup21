﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GobbyConversation : MonoBehaviour {

    public GameObject startConvo;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            startConvo.SetActive(true);
        }
    }
}