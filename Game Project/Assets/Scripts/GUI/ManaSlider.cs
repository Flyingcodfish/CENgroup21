using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaSlider : MonoBehaviour
{

	private int maxValue;
	private int value;
	public PlayerControl player;

	private Slider slider;

	// Use this for initialization
	void Start()
	{
		player = GameObject.FindWithTag("Player").GetComponent<PlayerControl>();
		slider = GetComponent<Slider>();
		maxValue = player.maxMana;
		slider.maxValue = maxValue;
		value = player.currentMana;
	}

	// Update is called once per frame
	void Update()
	{
		value = player.currentMana;
		slider.value = value;
	}
}
