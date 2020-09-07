using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : State
{
    public DeadState()
    {
        stateID = StateID.Die;
    }

    public override void DoBeforeEntering(Animator animator)
    {
        animator.Play("Die");
        GamePlayUI.instance.PlayEndGameSFX();
        GamePlayUI.instance.SetEndGameText("You won, good job!");
        GamePlayUI.instance.SetEndGamePanel(true);
    }
    public override void Act(Transform player)
    {

    }

    public override void Reason(Transform player)
    {

    }
}