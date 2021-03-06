﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopScript : MonoBehaviour {
    public inventory shopInventory;

    public CoinScript playerCoins;

    private PlayerControl player;

    // Use this for initialization
    public void Awake()
    {
       
    }
    public void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerControl>();
        shopInventory = GameObject.Find("Shop Inventory").GetComponent<inventory>();
        playerCoins = GameObject.Find("CoinHUD").GetComponent<CoinScript>();
        Manager.Instance.LoadSpecific("0-ARMOR-1;1-BOOTS-1;2-SWORD-1;3-HEALTH-1;4-MANA-1;5-SWIFT-1;6-POWER-1;7-STRENGTH-1;", "Shop Inventory"); // loads contents into specific inventory (contents,name)
        Debug.Log("Loaded");
        shopInventory.RenameSlots("Shop");
        Debug.Log("Renamed");
        shopInventory.SetShopItems();
        Debug.Log("Set");
    }
    public void Buy(Item item)
    {
        if(player.coins >= item.value) // if the player has enough coins to buy 
        {
            if (item.isUpgrade()) // if its a upgrade just use right away 
            {
                Debug.Log("is an upgrade");
                item.Use();
                playerCoins.MinusCoins(item.value);
            }
            else // else its a consumable so add to player inventory 
            {
                Debug.Log("Item is: " + item);
                player.inventory.AddItem(item);
                playerCoins.MinusCoins(item.value);
            }

        }
    }
	// Update is called once per frame
	void Update () {
        Vector2 pos = Camera.main.WorldToScreenPoint(this.transform.position); // gets position of obj in scene 
        shopInventory.transform.position = pos; // sets its position to always be on scene obj 
    }
}
