using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { MANA,HEALTH,SWIFT,STRENGTH,POWER}; // creates types for specific in game items 

public class Item : MonoBehaviour
{
    public ItemType type;

    public Sprite spriteNeutral, spriteHighlighted;

    private Actor player;

    public float speedTime = 20f, speedModifier = 2.0f;

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
                player.ModifyEffect(player, speedModifier, speedTime, type);
                break;
            case ItemType.STRENGTH:
                break;
            case ItemType.POWER:
                break;
        }
    }
}
