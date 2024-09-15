using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State
{
    public NavigateState navigateState;
    
    public IdleState idleState;

    public Transform anchor1;

    public Transform anchor2;

    public float vision = 10;  
    
    void GoToNextDestination()
    {
        float randomSpot = Random.Range(anchor1.position.x, anchor2.position.x);
        navigateState.destination = new Vector2(randomSpot, core.transform.position.y);
        Set(navigateState, true);
    }

    public override void Enter()
    {
        GoToNextDestination();
    }
    public override void Do()
    {
        if (TargetWithinVision() && TargetReachable())
        {
            if (machine.state == navigateState)
            {
                if (navigateState.isComplete)
                {
                    Set(idleState, true);
                    body.velocity = new Vector2(0, body.velocity.y);
                }
            }
            else
            {
                if (machine.state.time > 1)
                {
                    GoToNextDestination();
                }
            }
        }
        else 
        {
            Set(idleState, true);
            body.velocity = new Vector2(0, body.velocity.y);
        }
    }

    private bool TargetWithinVision()
    {
        //this needs to account for Y axis as well
        return Vector2.Distance(core.transform.position, navigateState.destination) < vision;
    }
    
    private bool TargetReachable()
    {
        return Mathf.Abs(core.transform.position.y - navigateState.destination.y) < 2;
    }
}