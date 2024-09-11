using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Core : MonoBehaviour
{
    public Rigidbody2D body;
    public Animator animator;
    public Ground ground;
    
    public StateMachine machine;

    public State state => machine.state;

    protected void Set(State newState, bool forceReset = false)
    {
        machine.Set(newState, forceReset);
    }

    public void SetupInstances()
    {
        machine = new StateMachine();
        Debug.Log("Setting up instances");
        // note: using GetComponents.. is costly, but here it only activates once.
        State[] allChildStates = GetComponentsInChildren<State>();
        foreach (State state in allChildStates)
        {
            state.SetCore(this);
        }
    }

    void OnDrawGizmos() 
    {
        #if UNITY_EDITOR
        if (Application.isPlaying && state != null)
        {
            List<State> states = machine.GetActiveStateBranch();
            UnityEditor.Handles.Label(transform.position, "Active States: " + string.Join(", ", states));
        }
        #endif
    }
}