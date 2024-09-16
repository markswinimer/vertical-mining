using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    public bool isComplete { get; protected set; }
    float startTime;
    public float time => Time.time - startTime;
    
    //I need to refactor core now that I have two kinds
    protected Core core;
    protected Rigidbody2D body => core.body;
    protected Animator animator => core.animator;

    public StateMachine machine;
    public State state => machine.state;

    //the state machine that called us
    protected StateMachine parent;

    protected void Set(State newState, bool forceReset = false)
    {
        machine.Set(newState, forceReset);
    }

    public void SetCore(Core _core)
    {
        machine = new StateMachine();
        core = _core;
    }
    
    public void Initialize(StateMachine _parent)
    {
        parent = _parent;
        isComplete = false;
        startTime = Time.time;
    }

    public virtual void Enter() {}
    public virtual void Do() {}
    public virtual void FixedDo() {}
    public virtual void Exit() {}

    //calls all of the "Do" functions in the active branch
    public void DoBranch()
    {
        Do();
        state?.DoBranch();
    }

    //calls all of the "FixedDo" functions in the active branch
    public void FixedDoBranch()
    {
        FixedDo();
        state?.FixedDoBranch();
    }
}