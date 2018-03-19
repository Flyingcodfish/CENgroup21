﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class inventory : MonoBehaviour {
    // used for inventory layout 
	private RectTransform inventoryRect;

	private float inventoryWidth, inventoryHeight;

	public int slots, rows;

	public float slotPaddingLeft, slotPaddingTop, slotSize;

    // used for moving stuff around and maintaining inventory 

	public GameObject slotPrefab;

    public GameObject iconPrefab;

    private static GameObject hoverObject;

    public Canvas canvas;

    private float hoverYOffset;

	private static slot from, to;

    public EventSystem eventSystem;

	private List<GameObject> allSlots;

	private static int emptySlots;

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


	// Use this for initialization
	void Start () {
		CreateLayout();
	}

	// Update is called once per frame
	void Update () { 
        if (Input.GetMouseButtonUp(0))
        {
            if (!eventSystem.IsPointerOverGameObject(-1) && from != null) // if mouse pointer not over game object 
            {
                from.GetComponent<Image>().color = Color.white;
                from.ClearSlot();
                Destroy(GameObject.Find("Hover"));
                to = null;
                from = null;
                hoverObject = null;
            }
        }
        if(hoverObject != null)
        {
            float xm = Input.mousePosition.x; // finds mouse position 
            float ym = Input.mousePosition.y;
            hoverObject.transform.position = new Vector2(xm + 1, ym + 1); // makes object follow mouse 
        }
        // use specific items based on which num is used 
        if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("1 is Pressed");
            allSlots[0].SendMessage("UseItem");
        }
        if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("2 is Pressed");
            allSlots[1].SendMessage("UseItem");
        }
        if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("3 is Pressed");
            allSlots[2].SendMessage("UseItem");
        }
        if (Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Alpha4))
        {
            Debug.Log("4 is Pressed");
            allSlots[3].SendMessage("UseItem");
        }
        if (Input.GetKeyDown(KeyCode.Keypad5) || Input.GetKeyDown(KeyCode.Alpha5))
        {
            Debug.Log("5 is Pressed");
            allSlots[4].SendMessage("UseItem");
        }
        // end hot bar keys 
    }
	private void CreateLayout() // creates the inventory layout based on fields and formulas 
	{
		allSlots = new List<GameObject>();

        hoverYOffset = slotSize * 0.01f;

		emptySlots = slots;

		inventoryWidth = (slots / rows) * (slotSize + slotPaddingLeft) + slotPaddingLeft;

		inventoryHeight = rows * (slotSize + slotPaddingTop) + slotPaddingTop;

		inventoryRect = GetComponent<RectTransform>();

		inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, inventoryWidth);

		inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, inventoryHeight);

		int columns = slots / rows;

		for(int y = 0; y < rows; y++)
		{
			for(int x = 0; x < columns; x++)
			{
				GameObject newSlot = (GameObject)Instantiate(slotPrefab);

				RectTransform slotRect = newSlot.GetComponent<RectTransform>();

				newSlot.name = "Slot";

				newSlot.transform.SetParent(this.transform.parent); // sets parent to canvas 

				slotRect.localPosition = inventoryRect.localPosition + new Vector3(slotPaddingLeft * (x + 1) + (slotSize * x), -slotPaddingTop * (y + 1) - (slotSize * y));

				slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize);
				slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize);

				allSlots.Add(newSlot);
			}
		}

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
					tmp.AddItem(item);
					emptySlots--;
					return true;
				}
			}
		}
		return false;
	}
	public void MoveItem(GameObject clicked) // first gets from object from first click then to object and moves the item stacks if possible 
	{
		if (from == null)
		{
			if (!clicked.GetComponent<slot>().IsEmpty)
			{
				from = clicked.GetComponent<slot>();
				from.GetComponent<Image>().color = Color.gray; // shows selected 

                hoverObject = (GameObject)Instantiate(iconPrefab);
                hoverObject.GetComponent<Image>().sprite = clicked.GetComponent<Image>().sprite;
                hoverObject.name = "Hover";

                RectTransform hoverTransform = hoverObject.GetComponent<RectTransform>();
                RectTransform clickedTransform = clicked.GetComponent<RectTransform>();

                hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, clickedTransform.sizeDelta.x);
                hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, clickedTransform.sizeDelta.y);

                hoverObject.transform.SetParent(GameObject.Find("HUDCanvas").transform, true);
                hoverObject.transform.localScale = from.gameObject.transform.localScale;
            }
		}
		else if(to == null)
		{
			to = clicked.GetComponent<slot>();
            Destroy(GameObject.Find("Hover"));
		}
		if(to != null && from != null) // switches the item stacks arround if one slot empty clears from and adds it to to 
		{
			Stack<Item> tmpTo = new Stack<Item>(to.Items);
			to.Additems(from.Items);

			if(tmpTo.Count == 0)
			{
				from.ClearSlot();
			}
			else
			{
				from.Additems(tmpTo);
			}

			from.GetComponent<Image>().color = Color.white; // resets color 
			to = null; // sets to null to allow for other objects to be moved 
			from = null;
            hoverObject = null;
		}
	}
}
