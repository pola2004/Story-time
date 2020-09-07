using UnityEngine;

public class IdleState : State
{
    bool canTranslate;
    public IdleState()
    {
        stateID = StateID.Idle;
        canTranslate = true;
    }

    public override void DoBeforeEntering(Animator animator)
    {
        animator.Play("Idle");
        startTime = Time.time;
        canTranslate = true;
    }
    public override void Act(Transform player)
    {

    }

    public override void Reason(Transform player)
    {
        if (Time.time < startTime + 1.5f)
            return;
        if (currentTransition == 0)
            currentTransition++;
        if ((currentTransition == 1 || currentTransition == 2 || currentTransition == 4 || currentTransition == 5 || currentTransition == 7) && canTranslate)
        {
            player.GetComponent<Character>().SetTransition(Transition.Walk, player.GetComponent<Animator>());
            currentTransition++;
            canTranslate = false;
            return;
        }
        if (currentTransition == 3 || currentTransition == 6)
        {
            GamePlayUI.instance.ShowAnswers(true);
            currentTransition++;
            canTranslate = false;
        }

    }
}