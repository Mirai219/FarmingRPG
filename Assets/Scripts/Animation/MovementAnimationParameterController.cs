using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementAnimationParameterContorller: MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        EventsHandler.MovementEventsHandler.MovementEvent += SetAnimationParameters;
    }
    private void OnDisable()
    {
        EventsHandler.MovementEventsHandler.MovementEvent -= SetAnimationParameters;
    }
    private void SetAnimationParameters
        (
            float inputX, float inputY,
            bool isWalking, bool isRunning, bool isIdle, bool isCarrying,
            ToolEffects toolEffects,
            bool isUsingToolRight, bool isUsingToolLeft, bool isUsingToolUp, bool isUsingToolDown,
            bool isLiftingToolRight, bool isLiftingToolLeft, bool isLiftingToolUp, bool isLiftingToolDown,
            bool isPickingRight, bool isPickingLeft, bool isPickingUp, bool isPickingDown,
            bool isSwingingToolRight, bool isSwingingToolLeft, bool isSwingingToolUp, bool isSwingingToolDown,
            bool idleUp, bool idleDown, bool idleLeft, bool idleRight
        )
    {
        animator.SetFloat(Settings.xInput,inputX);
        animator.SetFloat(Settings.yInput,inputY);
        animator.SetBool(Settings.isWalking,isWalking);
        animator.SetBool(Settings.isRunning,isRunning);
        animator.SetInteger(Settings.toolEffect,(int)toolEffects);

        if(isUsingToolRight) 
            animator.SetTrigger(Settings.isUsingToolRight);
        if(isUsingToolLeft)
            animator.SetTrigger (Settings.isUsingToolLeft);
        if (isUsingToolUp)
            animator.SetTrigger(Settings.isUsingToolUp);
        if (isUsingToolDown)
            animator.SetTrigger(Settings.isUsingToolDown);

        if(isLiftingToolDown)
            animator.SetTrigger(Settings.isLiftingToolDown);
        if (isLiftingToolUp)
            animator.SetTrigger(Settings.isLiftingToolUp);
        if (isLiftingToolLeft)
            animator.SetTrigger(Settings.isLiftingToolLeft) ;
        if (isLiftingToolRight)
            animator.SetTrigger(Settings.isLiftingToolRight);

        if (isSwingingToolDown)
            animator.SetTrigger(Settings.isSwingingToolDown);
        if (isSwingingToolUp)
            animator.SetTrigger(Settings.isSwingingToolUp);
        if (isSwingingToolLeft)
            animator.SetTrigger(Settings.isSwingingToolLeft);
        if (isSwingingToolRight)
            animator.SetTrigger(Settings.isSwingingToolRight);

        if (isPickingDown)
            animator.SetTrigger(Settings.isPickingDown);
        if (isSwingingToolUp)
            animator.SetTrigger(Settings.isPickingUp);
        if (isPickingLeft)
            animator.SetTrigger(Settings.isPickingLeft);
        if (isPickingRight)
            animator.SetTrigger(Settings.isPickingRight);

        if (idleDown)
            animator.SetTrigger(Settings.idleDown);
        if (idleUp)
            animator.SetTrigger(Settings.idleUp);
        if (idleLeft)
            animator.SetTrigger(Settings.idleLeft);
        if (idleRight)
            animator.SetTrigger(Settings.idleRight);

    }
    private void AnimationEventPlayFootstepSound()
    {
    
    }
}
