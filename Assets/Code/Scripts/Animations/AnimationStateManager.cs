using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateManager : MonoBehaviour
{
    public State currentState;
    public Animator animator;

    // Update is called once per frame
    void Update()
    {
        RunStateMachine();
    }

    public void PlayTargetAnimation(string targetAnim, bool isInteracting)
    {
        animator.applyRootMotion = isInteracting;
        animator.SetBool(currentState.name, isInteracting);
        animator.CrossFade(targetAnim, 0.2f);
    }

    private void RunStateMachine()
    {
        State nextState = currentState?.RunCurrentState();

        if (nextState != null)
        {
            //Switch to next state
            SwitchToNextState(nextState);

        }
    }

    private void SwitchToNextState(State nextState)
    {
        currentState = nextState;   
    }
}
