using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType {MANA,HEALTH,SWIFT,STRENGTH,POWER, SPELL_ICE, SPELL_FIRE, SPELL_PUSH, ARMOR, SHOP_HEALTH, SHOP_MANA}; // creates types for specific in game items 

public class Item : MonoBehaviour
{   
    public ItemType type;

    public Sprite spriteNeutral, spriteHighlighted;

    private PlayerControl player;
    // modifiers for each type

    public float speedTime = 20f, speedModifier = 2.0f;

    public float strengthTime = 15f, strengthModifier = 0.5f;

    public float powerTime = 5f, powerModifier = 1.5f;

    //
    // used for item descriptions and shop 
    public int value;

    public string description;

    public bool shopItem;

    public void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerControl>();
    }
    public int maxSize; // decides how large can stack

    public void Use()
    {
        switch (type)
        {
        case ItemType.HEALTH:
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
		case ItemType.SPELL_ICE:
			player.CastIce();
			break;
		case ItemType.SPELL_FIRE:
			player.CastFire();
			break;
		case ItemType.SPELL_PUSH:
			player.CastPush();
			break;
        }
    }
    public bool isSpell()
    {
		return (type == ItemType.SPELL_ICE || type == ItemType.SPELL_FIRE || type == ItemType.SPELL_PUSH);// just have or for each spell type 
    }

	//if this spell item has been picked up before, pretend it doesn't exist.
	public void Start(){
		if (type == ItemType.SPELL_ICE  && GameSaver.liveSave.spellTaken [0] ||
		    type == ItemType.SPELL_FIRE && GameSaver.liveSave.spellTaken [1] ||
		    type == ItemType.SPELL_PUSH && GameSaver.liveSave.spellTaken [2])
		{
			Destroy (this.gameObject);
		}
	}

    public bool isShop()
    {
       return  shopItem; 
    }
    public bool isUpgrade()
    {
        return (type == ItemType.ARMOR); // just have one for each upgrade type 
    }
}
    
