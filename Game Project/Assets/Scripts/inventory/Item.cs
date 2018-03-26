using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType {MANA,HEALTH,SWIFT,STRENGTH,POWER, SPELL_ICE, SPELL_FIRE}; // creates types for specific in game items 

public class Item : MonoBehaviour
{
    public ItemType type;

    public Sprite spriteNeutral, spriteHighlighted;

    private PlayerControl player;

    public float speedTime = 20f, speedModifier = 2.0f;

    public float strengthTime = 15f, strengthModifier = 0.5f;

	public float powerTime = 5f, powerModifier = 1.5f;

    public int maxSize;

    public void Use()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerControl>();
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
        }
    }
    public bool isSpell()
    {
		return (type == ItemType.SPELL_ICE || type == ItemType.SPELL_FIRE);// just have or for each spell type 
    }
}
