using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { MANA,HEALTH}; // creates types for specific in game items 

public class Item : MonoBehaviour {
    public ItemType type;

    public Sprite spriteNeutral, spriteHighlighted;

    public int maxSize;

	public void Use()
    {
        switch (type)
        {
            case ItemType.HEALTH:

                break;
            case ItemType.MANA:
                break;
        }
    }
}
