﻿using UnityEngine;

public class AttackBehavior : StateMachineBehaviour
{
	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		animator.SetBool("attacking", false);
	}
}
