using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttackState : StateMachineBehaviour {

	//normalized time points in attack animation.
	//hitbox should be active in these bounds; upper bound is exclusive
	//numbers calculated so that hitbox is active for 8th and 9th frames out of the animation's 18 frames
	float hitStart = 0.4444f;
	float hitEnd = 0.55555f;

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
