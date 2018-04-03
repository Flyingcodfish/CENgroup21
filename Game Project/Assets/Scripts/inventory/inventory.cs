using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class inventory : MonoBehaviour {
    // used for inventory layout 
	private RectTransform inventoryRect;

	private float inventoryWidth, inventoryHeight;

	public int slots, rows, spells;

	public float slotPaddingLeft, slotPaddingTop, slotSize;

    // used for moving stuff around and maintaining inventory 
    public GameObject spellSlotPrefab;

	public GameObject slotPrefab;

    public GameObject iconPrefab;

    private static GameObject hoverObject;

    public Canvas canvas;

    private float hoverYOffset;

	private static slot from, to;

    public EventSystem eventSystem;

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

    public GameObject mana,health,power,iceSpell,strength,swift;

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
    void Start () {
        isOpen = false; // defaults to inventory being closed
        canvasGroup = GetComponent<CanvasGroup>();// gets reference to specific canvas used for inv 
        hudGroup = transform.parent.GetComponent<CanvasGroup>();// gets reference to canvas group of HUD 
		CreateLayout();
	}

	// Update is called once per frame
	void Update () { 
        // used for removing items 
        if (Input.GetMouseButtonUp(0))
        {
            if (!eventSystem.IsPointerOverGameObject(-1) && from != null) // if mouse pointer not over game object 
            {
                from.GetComponent<Image>().color = Color.white;
                if (!from.Items.Peek().isSpell())// if its not a spell its safe to clear the slot 
                {
                    from.ClearSlot();
                    emptySlots++;
                }
                Destroy(GameObject.Find("Hover"));
                to = null;
                from = null;
            }
        }
        // hover object follows 
        if(hoverObject != null)
        {
            float xm = Input.mousePosition.x; // finds mouse position 
            float ym = Input.mousePosition.y;
            hoverObject.transform.position = new Vector2(xm + 1, ym + 1); // makes object follow mouse 
        }
        // used for debugging save and load **********************
        if (Input.GetKeyDown(KeyCode.K))
        {
            SaveInventory();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadInventory();
        }
        // ******************************************************
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
                    newSlot = (GameObject)Instantiate(spellSlotPrefab);
                    newSlot.name = "Spell";
                    count++;
                }
                else
                {
                    newSlot = (GameObject)Instantiate(slotPrefab);
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
		if (from == null && clicked.transform.parent.GetComponent<inventory>().isOpen) // can only  move if inventory shown, Open
		{
            Debug.Log("Clicked and is Open");
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
		if(to != null && from != null) // switches the item stacks arround if one slot empty clears from and adds it to to, only does so if the slots are same type 
		{
            if (to.GetComponent<slot>().name == from.GetComponent<slot>().name)
            {
                Stack<Item> tmpTo = new Stack<Item>(to.Items);
                to.Additems(from.Items);

                if (tmpTo.Count == 0)
                {
                    from.ClearSlot();
                }
                else
                {
                    from.Additems(tmpTo);
                }

            }
                from.GetComponent<Image>().color = Color.white; // resets color 
                to = null; // resets to null to allow for other objects to be moved and doesnt move anything since of different slot type
                from = null;
                Destroy(GameObject.Find("Hover"));

        }
    }
    private void PutItemBack() // if inventory faded when moving an item around 
    {
        if(from != null)
        {
            Destroy(GameObject.Find("Hover"));
            from.GetComponent<Image>().color = Color.white;
            from = null; // resets from 
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
   public void SaveInventory()
    {
        Debug.Log("Saving");
        string content = string.Empty;
        for(int i =0; i< allSlots.Count; i++) // goes through allslots and concatentates string with each index, type, and amount
        {
            slot tmp = allSlots[i].GetComponent<slot>();
            if (!tmp.IsEmpty)
            {
                content += i + "-" + tmp.CurrentItem.type.ToString() + "-" + tmp.Items.Count.ToString() + ";";
            }
        }

        PlayerPrefs.SetString("content", content);
        PlayerPrefs.SetInt("slots", slots);
        PlayerPrefs.SetInt("rows", rows);
        PlayerPrefs.SetFloat("slotPaddingLeft", slotPaddingLeft);
        PlayerPrefs.SetFloat("slotPaddingTop", slotPaddingTop);
        PlayerPrefs.SetFloat("slotSize", slotSize);
        PlayerPrefs.Save(); // saves all the data member fields in playerprefs to be used for load 
    }
    public void LoadInventory()
    {
        Debug.Log("Loading");
        string content = PlayerPrefs.GetString("content");
        slots = PlayerPrefs.GetInt("slots");
        rows = PlayerPrefs.GetInt("rows");
        slotPaddingLeft = PlayerPrefs.GetFloat("slotPaddingLeft");
        slotPaddingTop = PlayerPrefs.GetFloat("slotPaddingTop");
        slotSize = PlayerPrefs.GetFloat("slotSize");

        CreateLayout();

        string[] splitContent = content.Split(';'); // delims by each slot 

        for(int i=0; i < splitContent.Length - 1; i++)
        {
            string[] splitValues = splitContent[i].Split('-'); // delims by each subsection saved 
            int index = Int32.Parse(splitValues[0]); // gets slot number, index 
            ItemType type = (ItemType)Enum.Parse(typeof(ItemType), splitValues[1]); // gets item type 
            Debug.Log(splitValues[1]);
            int amount = Int32.Parse(splitValues[2]); // gets amount 
            for (int j = 0; j< amount; j++)
            {
                switch (type) // for the amount goes through and add items based on prefab member fields 
                {
                    case ItemType.MANA:
                        allSlots[index].GetComponent<slot>().AddItem(mana.GetComponent<Item>());
                        break;
                    case ItemType.HEALTH:
                        allSlots[index].GetComponent<slot>().AddItem(health.GetComponent<Item>());
                        break;
                    case ItemType.POWER:
                        allSlots[index].GetComponent<slot>().AddItem(power.GetComponent<Item>());
                        break;
                    case ItemType.SPELL_ICE:
                        allSlots[index].GetComponent<slot>().AddItem(iceSpell.GetComponent<Item>());
                        break;
                    case ItemType.SWIFT:
                        allSlots[index].GetComponent<slot>().AddItem(swift.GetComponent<Item>());
                        break;
                    case ItemType.STRENGTH:
                        allSlots[index].GetComponent<slot>().AddItem(strength.GetComponent<Item>());
                        break;
                }
            }

        }

    }
}
