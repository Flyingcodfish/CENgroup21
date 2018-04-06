using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopScript : MonoBehaviour {
    public inventory shopInventory;

    private PlayerControl player;

    // Use this for initialization
    public void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerControl>();
    }
    public void Buy(Item item)
    {
        if(player.coins > item.value) // if the player has enough coins to buy 
        {
            if (item.isShop()) // if its a upgrade just use right away 
            {
                item.Use();
            }
            else // else its a consumable so add to player inventory 
            {
                player.inventory.AddItem(item);
            }

        }
    }
	// Update is called once per frame
	void Update () {
		
	}
}
