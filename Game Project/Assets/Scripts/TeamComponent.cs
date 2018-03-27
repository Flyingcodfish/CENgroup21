using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamComponent : MonoBehaviour {
	//makes AI behaviors more flexible; allows projectiles to ignore friendlies

	public Team team = Team.neutral;

}

public enum Team {
	neutral, player, enemy, special
};

