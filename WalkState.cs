using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : State
{
    private int currentWayPoint;
    private Transform[] waypoints;
    private int targetWayPoint;

    private bool canTranslate;

    public float speed = 2.0f;

    public WalkState (Transform[] wayPoints)
    {
        this.waypoints = wayPoints;
        currentWayPoint = 0;
        stateID = StateID.Walk;

    }
    public override void DoBeforeEntering (Animator animator)
    {
        animator.Play ("Run");
        targetWayPoint = currentWayPoint + 1;
    }
    public override void DoBeforeLeaving (Animator animator)
    {
        currentWayPoint++;
    }
    public override void Reason (Transform rigidbody)
    {
        if ((currentWayPoint == 0 ||currentWayPoint == 1||currentWayPoint == 2||currentWayPoint == 3)&& canTranslate)
        {
            rigidbody.GetComponent<Character> ().SetTransition (Transition.Idle, rigidbody.GetComponent<Animator> ());
            canTranslate = false;
        }
  
        if (currentWayPoint == 4 && canTranslate)
        {
            rigidbody.GetComponent<Character> ().SetTransition (Transition.Die, rigidbody.GetComponent<Animator> ());
            canTranslate = false;
        }
    }

    public override void Act (Transform player)
    {

        if (player.position.x < waypoints[targetWayPoint].position.x)
        {

            player.position = Vector2.MoveTowards (player.position, waypoints[targetWayPoint].position, speed * Time.deltaTime);
        }
        if (player.position.x == waypoints[targetWayPoint].position.x)
        {
            canTranslate = true;
        }
    }

}