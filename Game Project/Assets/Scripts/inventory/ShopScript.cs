using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopScript : MonoBehaviour {
    public inventory shopInventory;

    public CoinScript playerCoins;

    private PlayerControl player;

    // Use this for initialization
    public void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerControl>();
    }
    public void Start()
    {
        Manager.Instance.LoadSpecific("1-MANA-1;2-HEALTH-1;0-ARMOR-1;", "Shop Inventory"); // loads contents into specific inventory (contents,name)
        shopInventory.RenameSlots("Shop");
        shopInventory.SetShopItems();
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
