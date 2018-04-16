using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour {

    public inventory chestInventory;

    private PlayerControl player;

    // Use this for initialization
    void Start() {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerControl>();
        chestInventory = GameObject.Find("Chest Inventory").GetComponent<inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 pos = Camera.main.WorldToScreenPoint(this.transform.position); // gets position of obj in scene 
        chestInventory.transform.position = pos; // sets its position to always be on scene obj 
    }
}
