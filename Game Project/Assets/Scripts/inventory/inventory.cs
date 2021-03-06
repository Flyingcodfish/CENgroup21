﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class inventory : MonoBehaviour {
    // used for inventory layout 
	private RectTransform inventoryRect;

	private float inventoryWidth, inventoryHeight;

	public int slots, rows, spells, chests;

	public float slotPaddingLeft, slotPaddingTop, slotSize;

    // used for moving stuff around and maintaining inventory 

    private float hoverYOffset;

	private List<GameObject> allSlots;

	private static int emptySlots; // made static to have only one instance of empty slots and ref without instance 
    // used for fading HUD elements in and out 
    private  CanvasGroup canvasGroup; 

    private static CanvasGroup hudGroup;

    private bool fadingIn;

    private bool fadingOut;

    public float fadeTime;

    // used for chests and vendors opening inventory

    private bool isOpen;

    // used for saving and loading 

    private Manager manager;

	public static int EmptySlots // used to get emptySlots in other scripts 
	{
		get
		{
			return emptySlots;
		}

		set
		{
			emptySlots = value;
		}
	}

    public static CanvasGroup HudGroup // used to get hud group in other scripts 
    {
        get
        {
            return hudGroup;
        }
        set
        {
            hudGroup = value;
        }
    }

    public bool IsOpen // used to get isOpen from other scripts 
    {
        get
        {
            return isOpen;
        }

        set
        {
            isOpen = value;
        }
    }


    // Use this for initialization
    void Awake() {
        isOpen = false; // defaults to inventory being closed
        canvasGroup = GetComponent<CanvasGroup>();// gets reference to specific canvas used for inv 
        hudGroup = transform.parent.GetComponent<CanvasGroup>();// gets reference to canvas group of HUD 
        manager = GameObject.Find("InventoryManager").GetComponent<Manager>();// gets reference to manager of inventories 
		CreateLayout();
	}

	// Update is called once per frame
	void Update () { 
        // used for removing items 
        if (Input.GetMouseButtonUp(0))
        {
            if (!Manager.Instance.eventSystem.IsPointerOverGameObject(-1) && Manager.Instance.From != null) // if mouse pointer not over game object 
            {
                Manager.Instance.From.GetComponent<Image>().color = Color.white;
                Debug.Log("Shop Item" + Manager.Instance.From.Shop);
                if (!Manager.Instance.From.Items.Peek().isSpell() && !Manager.Instance.From.Shop)// if its not a spell and shop item its safe to clear the slot 
                {
                    Manager.Instance.From.ClearSlot();
                    emptySlots++;
                }
                Destroy(GameObject.Find("Hover"));
                Manager.Instance.To = null;
                Manager.Instance.From = null;
            }
        }
        // hover object follows 
        if(Manager.Instance.HoverObject != null)
        {
            float xm = Input.mousePosition.x; // finds mouse position 
            float ym = Input.mousePosition.y;
            Manager.Instance.HoverObject.transform.position = new Vector2(xm + 1, ym + 1); // makes object follow mouse 
        }
    }
    public void Open()
    {
        // fade in/out  HUD elements
            if (canvasGroup.alpha > 0)
            {
                StartCoroutine("FadeOut",0);
                PutItemBack();

                isOpen = false; // inventory closes 
            }
            else
            {
                StartCoroutine("FadeIn",0);

                isOpen = true; // inventory opens 
            }
        
    }
    public void UseItem(int x)
    {
        allSlots[x].SendMessage("UseItem");
    }
    private void CreateLayout() // creates the inventory layout based on fields and formulas 
	{
        Debug.Log("Creating Layout");
        if(allSlots != null) // clears list if loading to not stack slots per load 
        {
            foreach(GameObject slot in allSlots)
            {
                Destroy(slot);
            }
        }

		allSlots = new List<GameObject>();
        
        hoverYOffset = slotSize * 0.01f;

		emptySlots = slots;

		inventoryWidth = (slots / rows) * (slotSize + slotPaddingLeft) + slotPaddingLeft; // calcs width 

		inventoryHeight = rows * (slotSize + slotPaddingTop) + slotPaddingTop; // calcs height 

		inventoryRect = GetComponent<RectTransform>();

		inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, inventoryWidth);

		inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, inventoryHeight);

		int columns = slots / rows; // calcs columns 

        int count = 0;

		for(int y = 0; y < rows; y++)
		{
			for(int x = 0; x < columns; x++)
			{
                GameObject newSlot;

                if (count < spells)// first slots for Spell items designated by member field 
                {
                    newSlot = (GameObject)Instantiate(Manager.Instance.spellSlotPrefab);
                    newSlot.GetComponent<slot>().useTxt.text = "[" + UseSlotNum(x, y).ToString() + "]";
                    newSlot.name = "Spell";
                    count++;
                }
                else if(count < chests) // else is a chest inventory 
                {
                    newSlot = (GameObject)Instantiate(Manager.Instance.chestSlotPrefab);
                    newSlot.name = "Slot";
                    count++;
                }
                else
                {
                    newSlot = (GameObject)Instantiate(Manager.Instance.slotPrefab);
                    newSlot.GetComponent<slot>().useTxt.text = "[" + UseSlotNum(x,y).ToString() + "]"; 
                    newSlot.name = "Slot";
                }

                RectTransform slotRect = newSlot.GetComponent<RectTransform>();

                newSlot.transform.SetParent(this.transform.parent); // sets parent to canvas 


				slotRect.localPosition = inventoryRect.localPosition + new Vector3(slotPaddingLeft * (x + 1) + (slotSize * x), -slotPaddingTop * (y + 1) - (slotSize * y));

				slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize);
				slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize); // multiply by canvas.scaleFactor to maybe allow scaling on diff screen sizes **not working**
                newSlot.transform.SetParent(this.transform); // sets parent to inventory 

				allSlots.Add(newSlot);
			}
		}

	}
    public void RenameSlots(string name) //used to rename slots to disable moving between slots/other identifiers 
    {
        if (allSlots != null)
        {
            foreach (GameObject slot in allSlots)
            {
                slot.name = name;
            }
        }
    }
    public void SetShopItems() // used to set items shopItem bool to true when in a shop 
    {
        if(allSlots != null)
        {
            foreach (GameObject slot in allSlots)
            {
                slot tmp = slot.GetComponent<slot>();
                if (!tmp.IsEmpty)
                {
                    tmp.Shop = true;
                    tmp.useTxt.text =  tmp.Items.Peek().value.ToString();
                    tmp.useTxt.color = Color.magenta;
                    tmp.useTxt.fontSize = 3;
                }
                else
                {
                    tmp.useTxt.text = string.Empty;
                }
            }
        }
    }
    private int UseSlotNum(int x, int y)
    {
        return (x + y + 1) >= 10 ? 0 : x + y + 1;
    }
	public bool AddItem(Item item)// inventory on player and then call when wanting to add item
	{
		if (item.maxSize == 1)
		{
			PlaceEmpty(item);
			return true;
		}
		else
		{
			foreach(GameObject slot in allSlots)
			{
				slot tmp = slot.GetComponent<slot>();
				if(!tmp.IsEmpty) 
				{
					if(tmp.CurrentItem.type == item.type && tmp.CanStack)
					{
						tmp.AddItem(item);
						return true;
					}
				}
			}
			if (emptySlots > 0)
			{
				PlaceEmpty(item);
			}
		}
		return false;
	}
	private bool PlaceEmpty(Item item) // finds next empty slot to place items in returns false if no slot 
	{
		if (emptySlots > 0)
		{
			foreach (GameObject slot in allSlots)
			{
				slot tmp = slot.GetComponent<slot>();
				if (tmp.IsEmpty)
				{
                    if (tmp.name == "Spell" && item.isSpell()) // checks if its a slot spell and item is a spell 
                    {
                        Debug.Log("slot is spell and item is spell");
                        if (SceneManager.GetActiveScene().name == "fire_dungeon" && !GameSaver.liveSave.bossKilled[0]) GameSaver.liveSave.firstFire = true;
                        if (SceneManager.GetActiveScene().name == "water_dungeon" && !GameSaver.liveSave.bossKilled[1]) GameSaver.liveSave.firstIce = true;
                        if (SceneManager.GetActiveScene().name == "maze_dungeon" && !GameSaver.liveSave.bossKilled[2]) GameSaver.liveSave.firstPush = true;
                        tmp.AddItem(item);
                        emptySlots--;
                        return true;
                    }
                    else if (tmp.name == "Slot" && !item.isSpell())
                    {
                        Debug.Log("slot is slot and item is not spell");
                        tmp.AddItem(item);
                        emptySlots--;
                        return true;
                    }
				}
			}
		}
		return false;
	}
	public void MoveItem(GameObject clicked) // first gets from object from first click then to object and moves the item stacks if possible 
	{
		if (Manager.Instance.From == null && clicked.transform.parent.GetComponent<inventory>().isOpen) // can only  move if inventory shown, Open
		{
            Debug.Log("Clicked and is Open");
			if (!clicked.GetComponent<slot>().IsEmpty)
			{
				Manager.Instance.From = clicked.GetComponent<slot>();
				Manager.Instance.From.GetComponent<Image>().color = Color.gray; // shows selected 

                Manager.Instance.HoverObject = (GameObject)Instantiate(Manager.Instance.iconPrefab);
                Manager.Instance.HoverObject.GetComponent<Image>().sprite = clicked.GetComponent<Image>().sprite;
                Manager.Instance.HoverObject.name = "Hover";

                RectTransform hoverTransform = Manager.Instance.HoverObject.GetComponent<RectTransform>();
                RectTransform clickedTransform = clicked.GetComponent<RectTransform>();

                hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, clickedTransform.sizeDelta.x);
                hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, clickedTransform.sizeDelta.y);

                Manager.Instance.HoverObject.transform.SetParent(GameObject.Find("HUDCanvas").transform, true);
                Manager.Instance.HoverObject.transform.localScale = Manager.Instance.From.gameObject.transform.localScale;
            }
		}
		else if(Manager.Instance.To == null)
		{
			Manager.Instance.To = clicked.GetComponent<slot>();
            Destroy(GameObject.Find("Hover"));
		}
		if(Manager.Instance.To != null && Manager.Instance.From != null) // switches the item stacks arround if one slot empty clears from and adds it to to, only does so if the slots are same type 
		{
            if (Manager.Instance.To.GetComponent<slot>().name == Manager.Instance.From.GetComponent<slot>().name)
            {
                Stack<Item> tmpTo = new Stack<Item>(Manager.Instance.To.Items);
                Manager.Instance.To.Additems(Manager.Instance.From.Items);

                if (tmpTo.Count == 0)
                {
                    Manager.Instance.From.ClearSlot();
                }
                else
                {
                    Manager.Instance.From.Additems(tmpTo);
                }

            }
                Manager.Instance.From.GetComponent<Image>().color = Color.white; // resets color 
                Manager.Instance.To = null; // resets to null to allow for other objects to be moved and doesnt move anything since of different slot type
                Manager.Instance.From = null;
                Destroy(GameObject.Find("Hover"));

        }
    }
    private void PutItemBack() // if inventory faded when moving an item around 
    {
        if(Manager.Instance.From != null)
        {
            Destroy(GameObject.Find("Hover"));
            Manager.Instance.From.GetComponent<Image>().color = Color.white;
            Manager.Instance.From = null; // resets from 
        }
    }
    public IEnumerator FadeOut()
    {
        if (!fadingOut)
        {
            fadingOut = true;
            fadingIn = false;
            StopCoroutine("FadeIn");

            float startAlpha = canvasGroup.alpha;// gets current alpha value of canvas group 

            float rate = 1.0f / fadeTime;

            float progress = 0.0f;

            while (progress < 1.0)
            {
                canvasGroup.alpha = Mathf.Lerp(startAlpha, 0, progress); // makes alpha linearly interpolated from start to 0 by progress
                progress += rate * Time.deltaTime;
                yield return null;
            }

            canvasGroup.alpha = 0;
            fadingOut = false;
        }
    }
    public IEnumerator FadeIn()
    {
        if (!fadingOut)
        {
            fadingOut = false;
            fadingIn = true;
            StopCoroutine("FadeOut");

            float startAlpha = canvasGroup.alpha;// gets current alpha value of canvas group 

            float rate = 1.0f / fadeTime;

            float progress = 0.0f;

            while (progress < 1.0)
            {
                canvasGroup.alpha = Mathf.Lerp(startAlpha, 1, progress);// makes alpha linearly interpolated from start to 1 by progress
                progress += rate * Time.deltaTime;
                yield return null;
            }

            canvasGroup.alpha = 1;
            fadingIn = false;
        }
    }
   public void SaveInventory() // uses gameObject.name to differentiate inventories in the scene
    {
        Debug.Log("Saving Inventory...");
        string content = string.Empty;
        for(int i =0; i< allSlots.Count; i++) // goes through allslots and concatentates string with each index, type, and amount
        {
            slot tmp = allSlots[i].GetComponent<slot>();
            if (!tmp.IsEmpty)
            {
                content += i + "-" + tmp.CurrentItem.type.ToString() + "-" + tmp.Items.Count.ToString() + ";";
            }
        }
       // Debug.Log("Contents is: " + content);
		GameSaver.liveSave.playerInventoryData.content = content;
        Debug.Log("Set Content is: " + content);
		GameSaver.liveSave.playerInventoryData.slots = slots;
		GameSaver.liveSave.playerInventoryData.rows = rows;
		GameSaver.liveSave.playerInventoryData.slotPaddingLeft = slotPaddingLeft;
		GameSaver.liveSave.playerInventoryData.slotPaddingTop = slotPaddingTop;
		GameSaver.liveSave.playerInventoryData.slotSize = slotSize;
    }
    public void LoadInventory(string arg = "") // uses gameObject.name to differentiate inventories in the scene, and arg string to load specific contents
    {
        Debug.Log("Loading inventory...");
		string content = GameSaver.liveSave.playerInventoryData.content;
        if(arg != string.Empty)
        { // need to format string like: index-Type-amount;
            content = arg;
        }
		slots = GameSaver.liveSave.playerInventoryData.slots;
		rows = GameSaver.liveSave.playerInventoryData.rows;
		slotPaddingLeft = GameSaver.liveSave.playerInventoryData.slotPaddingLeft;
		slotPaddingTop = GameSaver.liveSave.playerInventoryData.slotPaddingTop;
		slotSize = GameSaver.liveSave.playerInventoryData.slotSize;
        CreateLayout();

        string[] splitContent = content.Split(';'); // delims by each slot 
        Debug.Log("Loaded Content is: " + content);
        for (int i=0; i < splitContent.Length - 1; i++)
        {
            string[] splitValues = splitContent[i].Split('-'); // delims by each subsection saved 
            int index = Int32.Parse(splitValues[0]); // gets slot number, index 
            ItemType type = (ItemType)Enum.Parse(typeof(ItemType), splitValues[1]); // gets item type 
            Debug.Log(splitValues[1]);
            int amount = Int32.Parse(splitValues[2]); // gets amount 
            for (int j = 0; j< amount; j++)
            {
               // Debug.Log("Loading Slot: "+ i +" With " + j);
                switch (type) // for the amount goes through and add items based on prefab member fields 
                {
                    case ItemType.MANA:
                        allSlots[index].GetComponent<slot>().AddItem(Manager.Instance.mana.GetComponent<Item>());
                        break;
                    case ItemType.HEALTH:
                        allSlots[index].GetComponent<slot>().AddItem(Manager.Instance.health.GetComponent<Item>());
                        break;
                    case ItemType.POWER:
                        allSlots[index].GetComponent<slot>().AddItem(Manager.Instance.power.GetComponent<Item>());
                        break;
                    case ItemType.SPELL_ICE:
                        allSlots[index].GetComponent<slot>().AddItem(Manager.Instance.iceSpell.GetComponent<Item>());
                        break;
                    case ItemType.SWIFT:
                        allSlots[index].GetComponent<slot>().AddItem(Manager.Instance.swift.GetComponent<Item>());
                        break;
                    case ItemType.STRENGTH:
                        allSlots[index].GetComponent<slot>().AddItem(Manager.Instance.strength.GetComponent<Item>());
                        break;
                    case ItemType.ARMOR:
                        allSlots[index].GetComponent<slot>().AddItem(Manager.Instance.armor.GetComponent<Item>());
                        break;
                    case ItemType.SWORD:
                        allSlots[index].GetComponent<slot>().AddItem(Manager.Instance.sword.GetComponent<Item>());
                        break;
                    case ItemType.BOOTS:
                        allSlots[index].GetComponent<slot>().AddItem(Manager.Instance.boots.GetComponent<Item>());
                        break;
					case ItemType.SPELL_FIRE:
						allSlots[index].GetComponent<slot>().AddItem(Manager.Instance.fireSpell.GetComponent<Item>());
						break;
					case ItemType.SPELL_PUSH:
						allSlots[index].GetComponent<slot>().AddItem(Manager.Instance.pushSpell.GetComponent<Item>());
						break;
                }
            }
        }
    }
}
