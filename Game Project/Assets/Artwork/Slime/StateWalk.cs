using UnityEngine;

public class StateWalk : StateMachineBehaviour{

	//the start and end of a slime's mid-bounce air-time in normalized time
	float airStart = 0.333f;
	float airEnd = 0.777f;

	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerindex){
		float nTime = stateInfo.normalizedTime % 1;
		animator.SetBool("Grounded", nTime < airStart || nTime > airEnd);
	}
}
