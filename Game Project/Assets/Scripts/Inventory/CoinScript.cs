using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CoinType { LargeCoin };  // have different types of coins for variety 

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
        coinText.text = "Coins: " + player.coins.ToString();
    }
    public void AddCoins(CoinType coin) // easy add different coin values with additional cases 
    {
        Debug.Log("Adding Coins");
        switch (coin)
        {
            case CoinType.LargeCoin:
                player.coins += 50;
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
