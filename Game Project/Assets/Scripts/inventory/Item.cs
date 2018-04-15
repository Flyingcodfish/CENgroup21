using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType {MANA,HEALTH,SWIFT,STRENGTH,POWER, SPELL_ICE, SPELL_FIRE, ARMOR,SWORD,BOOTS}; // creates types for specific in game items 

public class Item : MonoBehaviour
{   
    public ItemType type;

    public Sprite spriteNeutral, spriteHighlighted;

    private PlayerControl player;

    public int maxSize; // decides how large can stack
    // modifiers for each type

    public float speedTime = 20f, speedModifier = 2.0f;

    public float strengthTime = 15f, strengthModifier = 0.5f;

    public float powerTime = 5f, powerModifier = 1.5f;

    //
    // used for item descriptions and shop 
    public int value;

    public string description;

    public void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerControl>();
    }

    public void Use()
    {
        Debug.Log("player is: " + player);
        if (player == null) // makes sure the player is not null when using, if loading during gameplay
        {
            player = GameObject.FindWithTag("Player").GetComponent<PlayerControl>();
        }
        switch (type)
        {
        case ItemType.HEALTH: // Potion uses 
			player.TakeDamage(-50);
            break;
        case ItemType.MANA:
			player.SpendMana(-50);
            break;
        case ItemType.SWIFT:
			player.ModifyEffect(Actor.Effect.SpeedUp, speedTime, speedModifier);
            break;
        case ItemType.STRENGTH:
			player.ModifyEffect(Actor.Effect.StrengthUp, strengthTime, strengthModifier);
            break;
        case ItemType.POWER:
			player.ModifyEffect(Actor.Effect.PowerUp, powerTime, powerModifier);
            break;
		case ItemType.SPELL_ICE: // Spell uses 
			player.CastIce();
			break;
		case ItemType.SPELL_FIRE:
			player.CastFire();
			break;
        case ItemType.ARMOR: // Upgrade uses
                player.AddStrength(0.1f); 
            break;
        case ItemType.SWORD:
                Debug.Log("SWORD");
                player.AddPower(0.1f);
            break;
        case ItemType.BOOTS:
                player.AddSpeed(50f);
            break;

        }
    }
    public bool isSpell()
    {
		return (type == ItemType.SPELL_ICE || type == ItemType.SPELL_FIRE);// just have or for each spell type 
    }
    public bool isUpgrade()
    {
        return (type == ItemType.ARMOR || type == ItemType.SWORD || type == ItemType.BOOTS); // just have one for each upgrade type 
    }
}
    
