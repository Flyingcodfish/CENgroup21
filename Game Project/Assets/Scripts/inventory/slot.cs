﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class slot : MonoBehaviour, IPointerClickHandler {
	private Stack<Item> items;

    private CanvasGroup canvasGroup;

    private ShopScript shop;

    private bool shopItem;

    public bool Shop
    {
        get { return shopItem; }
        set { shopItem = value; }
    }

    public Text stackTxt;

    public Text useTxt;

	public Sprite slotEmpty;
	public Sprite slotHighlight;
	// Use this for initialization
	public bool IsEmpty
	{
		get { return Items.Count == 0; } // checks if slot is empty 
	}

	public Item CurrentItem
	{
		get { return Items.Peek(); }
	}

	public bool CanStack // checks if possible to stack 
	{
		get { return CurrentItem.maxSize > Items.Count; }
	}

	public Stack<Item> Items // used to get stacks in other classes 
	{
		get
		{
			return items;
		}

		set
		{
			items = value;
		}
	}

	
    void Awake()
    {
        Items = new Stack<Item>();// instantiates items upon awake 
        if (GameObject.FindObjectOfType<ShopScript>())
        {
            shop = GameObject.FindObjectOfType<ShopScript>().GetComponent<ShopScript>();
        }
        // gets shop script from shop in scene if there 
    }

	void Start () {
		RectTransform slotRect = GetComponent<RectTransform>();
		RectTransform txtRect = stackTxt.GetComponent<RectTransform>();
        

		int txtScaleFactor = (int)(slotRect.sizeDelta.x * 0.60);
		stackTxt.resizeTextMaxSize = txtScaleFactor;
		stackTxt.resizeTextMinSize = txtScaleFactor;

		txtRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotRect.sizeDelta.y);
		txtRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotRect.sizeDelta.x);

        if(useTxt != null) // only shown and calculated if the slots have a definition of UseTxt and only happens for regular slots 
        {
            RectTransform useRect = useTxt.GetComponent<RectTransform>();
            useTxt.resizeTextMaxSize = txtScaleFactor;
            useTxt.resizeTextMinSize = txtScaleFactor;

            useRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotRect.sizeDelta.y);
            useRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotRect.sizeDelta.x);
        }

        if (transform.parent != null) // makes canvasGroup the same as parent if regular slot 
        {
            canvasGroup = transform.parent.GetComponent<CanvasGroup>();
        }

	}

	// Update is called once per frame
	void Update () {

	}

	public void AddItem(Item item)
	{
		Items.Push(item);
		if (Items.Count > 1)
		{
			stackTxt.text = Items.Count.ToString();
		}
		ChangeSprite(item.spriteNeutral, item.spriteHighlighted);
	}

	public void Additems(Stack<Item> items)
    
	{
		this.Items = new Stack<Item>(items);

		stackTxt.text = items.Count > 1 ? items.Count.ToString() : string.Empty;

		ChangeSprite(CurrentItem.spriteNeutral, CurrentItem.spriteHighlighted);
	}

	private void ChangeSprite(Sprite neutral,Sprite highlight)
	{
		GetComponent<Image>().sprite = neutral;

		SpriteState st = new SpriteState();

		st.highlightedSprite = highlight;
		st.pressedSprite = neutral;

		GetComponent<Button>().spriteState = st;
	}
	private void UseItem()
	{
		if (!IsEmpty)
		{
            if (Items.Peek().isSpell())
            {
                Items.Peek().Use();
            }
            else if (shopItem)// goes into shop options if shop item 
            {
                Debug.Log("About to Buy");
                shop.Buy(Items.Peek());
            }
            else
            {
                Items.Pop().Use();
                stackTxt.text = Items.Count > 1 ? Items.Count.ToString() : string.Empty;

                if (IsEmpty)
                {
                    ChangeSprite(slotEmpty, slotHighlight);
                    inventory.EmptySlots++;
                }
            }
		}
	}
	public void OnPointerClick(PointerEventData eventData)
	{
		if(eventData.button == PointerEventData.InputButton.Right && !GameObject.Find("Hover")
            && canvasGroup.alpha>0 )// can only use when not moving items and when hud is showing 
		{
			UseItem();
		}
	}
	public void ClearSlot()
	{
		items.Clear();
		ChangeSprite(slotEmpty, slotHighlight);
		stackTxt.text = string.Empty;
	}
}
