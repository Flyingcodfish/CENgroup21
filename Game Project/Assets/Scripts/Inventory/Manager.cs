using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
    public void Save()
    {
        GameObject[] inventories = GameObject.FindGameObjectsWithTag("Inventory");
        foreach (GameObject inventory in inventories)
        {
            inventory.GetComponent<inventory>().SaveInventory();
        }
    }
    public void Load()
    {
        GameObject[] inventories = GameObject.FindGameObjectsWithTag("Inventory");
        foreach(GameObject inventory in inventories)
        {
            inventory.GetComponent<inventory>().LoadInventory();
        }
    }
}
