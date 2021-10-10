using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAnimation : StateMachineBehaviour
{
    float passedTime;
    public float hiddenTime;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        passedTime = 0;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        passedTime += Time.deltaTime;
        if(passedTime >= hiddenTime)
        {
            animator.SetTrigger("HiddenIdle_" + Random.Range(0,2));
            passedTime = 0;
        }
    }
}
