using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : StateMachineBehaviour {

	//normalized time points in attack animation.
	//hitbox should be active in these bounds; upper bound is exclusive
	public float hitStart;
	public float hitEnd;

	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerindex){
		if (stateInfo.normalizedTime >= hitStart){
			animator.SetBool("AttackActive", true);
		}
		if (stateInfo.normalizedTime >= hitEnd){
			animator.SetBool("AttackActive", false);
		}
	}

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerindex){
		animator.SetBool("Attacking", false);
	}

}
