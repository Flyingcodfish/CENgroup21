using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { MANA,HEALTH,SWIFT,STRENGTH,POWER,SPELL}; // creates types for specific in game items 

public class Item : MonoBehaviour
{
    public ItemType type;

    public Sprite spriteNeutral, spriteHighlighted;

    private Actor player;

    public float speedTime = 20f, speedModifier = 2.0f;

    public float strengthTime = 15f, strengthModifier = 0.5f;

	public float powerTime = 5f, powerModifier = 1.5f;

    public int maxSize;

    public void Use()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Actor>();
        switch (type)
        {
            case ItemType.HEALTH:
                player.TakeDamage(-50);
                break;
            case ItemType.MANA:
                break;
            case ItemType.SWIFT:
                player.ModifyEffect(player, speedModifier, speedTime, "SWIFT");
                break;
            case ItemType.STRENGTH:
                player.ModifyEffect(player, strengthModifier, strengthTime, "STRENGTH");
                break;
            case ItemType.POWER:
			player.ModifyEffect(player, powerModifier, powerTime, "POWER");
                break;
            case ItemType.SPELL:
                Debug.Log("USING A SPELL");
                break;
        }
    }
    public bool isSpell()
    {
        return type == ItemType.SPELL;
    }
}
