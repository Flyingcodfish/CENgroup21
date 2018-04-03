using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CoinType { LargeCoin };  // have different types of coins for variety 

public class CoinScript : MonoBehaviour {
    public CoinType type;

    private static int coins;

    public Text coinText;
    // Use this for initialization
    void Start() {
        coins = 0;
        SetText();
    }
    private void SetText(){
        coinText.text = "Coins: " + coins.ToString();
    }
    public void AddCoins(CoinType coin)
    {
        switch (coin)
        {
            case CoinType.LargeCoin:
                coins += 500;
                break;
        }
    }
}
