using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSlider : MonoBehaviour
{

    private int maxValue;
    private int value;
    public Player player;

    private Slider slider;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        slider = GetComponent<Slider>();
        maxValue = player.maxHealth;
        slider.maxValue = maxValue;
        value = player.getHealth();
    }

    // Update is called once per frame
    void Update()
    {
        value = player.getHealth();
        slider.value = value;
    }
}
