using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CoinType { Large, Medium, Small };  // have different types of coins for variety 

public class CoinScript : MonoBehaviour {
    public CoinType type;

    private PlayerControl player;

    public Text coinText;
    // Use this for initialization
    void Start() {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerControl>();
        coinText = GameObject.Find("CoinText").GetComponent<Text>();
        player.coins = 0;
        SetText();
    }
    private void SetText(){
        coinText.text =  player.coins.ToString();
    }
    public void AddCoins(CoinType coin) // easy add different coin values with additional cases 
    {
        Debug.Log("Adding Coins");
        switch (coin)
        {
            case CoinType.Large:
                player.coins += 50;
                break;
			case CoinType.Medium:
				player.coins += 20;
				break;
			case CoinType.Small:
				player.coins += 10;
				break;
        }
        SetText();
    }
    public void MinusCoins(int value)
    {
        player.coins -= value;
        SetText();
    }
}
