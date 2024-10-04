using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Core : MonoBehaviour
{
    public Rigidbody2D body;
    public Animator animator;
    public Ground ground;
    public bool isActive;
    public StateMachine machine;

    public State state => machine.state;

    protected void Set(State newState, bool forceReset = false)
    {
        machine.Set(newState, forceReset);
    }

    public void SetupInstances()
    {
        machine = new StateMachine();
        // note: using GetComponents.. is costly, but here it only activates once.
        State[] allChildStates = GetComponentsInChildren<State>();
        foreach (State state in allChildStates)
        {
            state.SetCore(this);
        }
    }

    public virtual void EnterState() {
        isActive = true;
    }
    public virtual void ExitState() {
        isActive = false;
    }
}