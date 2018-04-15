using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Manager : MonoBehaviour { // used to manage all inventories and prefabs required for each 
    private static Manager instance;

    // properties declared to access private fields from other classes 
    public static Manager Instance
    {
        get
        {
            if(instance == null) // if hasnt been set, sets to the manager in the scene 
            {
                instance = FindObjectOfType<Manager>();
            }
            return instance;
        }
    }

    public slot From
    {
        get
        {
            return from;
        }

        set
        {
            from = value;
        }
    }

    public slot To
    {
        get
        {
            return to;
        }

        set
        {
            to = value;
        }
    }

    public GameObject HoverObject
    {
        get
        {
            return hoverObject;
        }

        set
        {
            hoverObject = value;
        }
    }
    // end of properties 
    // shared member variables of each inventory, placed here for easy management 

    public EventSystem eventSystem;

    public Canvas canvas;

    public GameObject spellSlotPrefab;

    public GameObject slotPrefab;

    public GameObject iconPrefab;

    private slot from, to;

    private GameObject hoverObject;

    public GameObject mana, health, power, iceSpell, strength, swift,armor; // types of itemPrefabs available // add sjop_mana item to test shop function******** START HERE 

    // Use this for initialization
    void Start () {
		
	}
    public void Save() // finds all inventories in the scene and saves them 
    {
        GameObject[] inventories = GameObject.FindGameObjectsWithTag("Inventory");
        foreach (GameObject inventory in inventories)
        {
            inventory.GetComponent<inventory>().SaveInventory();
        }
    }
    public void Load() // finda all inventories in the scene and loads them 
    {
        GameObject[] inventories = GameObject.FindGameObjectsWithTag("Inventory");
        foreach(GameObject inventory in inventories)
        {
            inventory.GetComponent<inventory>().LoadInventory(string.Empty); // loads from PlayerPref
        }
    }
    public void LoadSpecific(string contents,string name)
    {
        Debug.Log("Name is: " + name);
        GameObject specificInventory = GameObject.Find(name);
        specificInventory.GetComponent<inventory>().LoadInventory(contents); // loads the contents into the specific inventory 
    }
    // USED FOR DEBUGGING 
    public void TestLoad()
    {
        string contents = "4-MANA-2;1-SPELL_ICE-1;2-SPELL_ICE-1;";
        string name = "Shop Inventory";
        LoadSpecific(contents, name);
    }
}
