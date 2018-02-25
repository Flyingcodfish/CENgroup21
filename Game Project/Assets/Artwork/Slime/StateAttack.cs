using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateAttack : StateMachineBehaviour {

	//normalized time points in attack animation.
	//hitbox should be active in these bounds; upper bound is exclusive
	float hitStart = 0.5f;
	float hitEnd = 0.8f;

	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerindex){
		if (stateInfo.normalizedTime >= hitStart){
			animator.SetBool("AttackActive", true);
		}
		if (stateInfo.normalizedTime >= hitEnd){
			animator.SetBool("AttackActive", false);
		}
	}
}
