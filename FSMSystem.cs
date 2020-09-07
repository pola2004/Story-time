using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Transition
{
    NullTransition = 0,
    Idle,
    Walk,
    Jump,
    Die
}

public enum StateID
{
    NullStateID = 0,
    Idle,
    Walk,
    Jump,
    Die
}

public class FSMSystem
{
    private List<State> states;
    private StateID currentStateID;
    public StateID CurrentStateID { get { return currentStateID; } }
    private State currentState;
    public State CurrentState { get { return currentState; } }

    public FSMSystem ()
    {
        states = new List<State> ();
    }

    public void AddState (State s)
    {
        // Check for Null reference before deleting
        if (s == null)
        {
            Debug.LogError ("FSM ERROR: Null reference is not allowed");
        }

        // First State inserted is also the Initial state,
        //   the state the machine is in when the simulation begins
        if (states.Count == 0)
        {
            states.Add (s);
            currentState = s;
            currentStateID = s.ID;
            return;
        }

        // Add the state to the List if it's not inside it
        foreach (State state in states)
        {
            if (state.ID == s.ID)
            {
                Debug.LogError ("FSM ERROR: Impossible to add state " + s.ID.ToString () +
                    " because state has already been added");
                return;
            }
        }
        states.Add (s);
    }

    public void DeleteState (StateID id)
    {
        // Check for NullState before deleting
        if (id == StateID.NullStateID)
        {
            Debug.LogError ("FSM ERROR: NullStateID is not allowed for a real state");
            return;
        }

        // Search the List and delete the state if it's inside it
        foreach (State state in states)
        {
            if (state.ID == id)
            {
                states.Remove (state);
                return;
            }
        }
        Debug.LogError ("FSM ERROR: Impossible to delete state " + id.ToString () +
            ". It was not on the list of states");
    }

    public void PerformTransition (Transition trans, Animator animator)
    {
        // Check for NullTransition before changing the current state
        if (trans == Transition.NullTransition)
        {
            Debug.LogError ("FSM ERROR: NullTransition is not allowed for a real transition");
            return;
        }

        // Check if the currentState has the transition passed as argument
        StateID id = currentState.GetOutputState (trans);
        if (id == StateID.NullStateID)
        {
            Debug.LogError ("FSM ERROR: State " + currentStateID.ToString () + " does not have a target state " +
                " for transition " + trans.ToString ());
            return;
        }

        // Update the currentStateID and currentState		
        currentStateID = id;
        foreach (State state in states)
        {
            if (state.ID == currentStateID)
            {
                // Do the post processing of the state before setting the new one
                currentState.DoBeforeLeaving (animator);

                currentState = state;

                // Reset the state to its desired condition before it can reason or act
                currentState.DoBeforeEntering (animator);
                break;
            }
        }

    }

}