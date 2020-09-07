using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected Dictionary<Transition, StateID> map = new Dictionary<Transition, StateID> ();
    protected StateID stateID;
    protected int currentTransition;
    protected float startTime;
    public StateID ID { get { return stateID; } }

    public void AddTransition (Transition trans, StateID id)
    {
        // Check if anyone of the args is invalid
        if (trans == Transition.NullTransition)
        {
            Debug.LogError ("FSMState ERROR: NullTransition is not allowed for a real transition");
            return;
        }

        if (id == StateID.NullStateID)
        {
            Debug.LogError ("FSMState ERROR: NullStateID is not allowed for a real ID");
            return;
        }

        // Since this is a Deterministic FSM,
        //   check if the current transition was already inside the map
        if (map.ContainsKey (trans))
        {
            Debug.LogError ("FSMState ERROR: State " + stateID.ToString () + " already has transition " + trans.ToString () +
                "Impossible to assign to another state");
            return;
        }

        map.Add (trans, id);
    }

    public void DeleteTransition (Transition trans)
    {
        // Check for NullTransition
        if (trans == Transition.NullTransition)
        {
            Debug.LogError ("FSMState ERROR: NullTransition is not allowed");
            return;
        }

        // Check if the pair is inside the map before deleting
        if (map.ContainsKey (trans))
        {
            map.Remove (trans);
            return;
        }
        Debug.LogError ("FSMState ERROR: Transition " + trans.ToString () + " passed to " + stateID.ToString () +
            " was not on the state's transition list");
    }

    public StateID GetOutputState (Transition trans)
    {
        // Check if the map has this transition
        if (map.ContainsKey (trans))
        {
            return map[trans];
        }
        return StateID.NullStateID;
    }

    public virtual void DoBeforeEntering (Animator animator) { }

    public virtual void DoBeforeLeaving (Animator animator) { }

    public abstract void Reason (Transform player);

    public abstract void Act (Transform player);

}