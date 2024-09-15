using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectState : State
{
    public List<Transform> collectables;
    public Transform target;
    public NavigateState navigateState;
    public IdleState idleState;
    public float collectRadius = .5f;
    public float vision = 1;

    // check for collectable objects
    // if you find one, go to it
    // idle for a second after collect

    public override void Enter()
    {
        navigateState.destination = target.position;
        Set(navigateState, true);
    }

    public override void Do()
    {
        if (state == navigateState)
        {
            ChaseTarget();
        }
        else
        {
            EndPursuit();
        }
    }

    void ChaseTarget()
    {
        if (IsWithinReach(target.position))
        {
            //if the target is close enough, stop and collect it
            Set(idleState, true);
            body.velocity = new Vector2(0, body.velocity.y);
            target.gameObject.SetActive(false);
        }
        else if (!IsInVision(target.position))
        {
            //if the target is out of vision, stop and idleState
            Set(idleState, true);
            body.velocity = new Vector2(0, body.velocity.y);
        }
        else
        {
            //otherwise, keep chasing
            navigateState.destination = target.position;
            Set(navigateState, true);
        }
    }

    void EndPursuit()
    {
        if (state.time > 2)
        {
            // exit this state
            isComplete = true;
        }
    }

    public override void Exit()
    {

    }

    public bool IsWithinReach(Vector2 targetPos)
    {
        return Vector2.Distance(core.transform.position, targetPos) < collectRadius;
    }

    public bool IsInVision(Vector2 targetPos)
    {
        return Vector2.Distance(core.transform.position, targetPos) < vision;
    }

    public void CheckForTarget()
    {
        foreach (Transform ore in collectables)
        {
            if (IsInVision(ore.position) && ore.gameObject.activeSelf)
            {
                target = ore;
                return;
            }
        }

        target = null;
    }
}
