using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleWanderState : State
{
    public FlyingNavigateState flyingNavigateState;
    public IdleFlyingState idleFlyingState;

    public float rotationSpeed = 100f;

    public AnimationClip clip;

    public float vision = 100f;

    public override void Enter()
    {
        GoToNextDestination();
        animator.Play(clip.name);
    }

    void GoToNextDestination()
    {
        // Generate a random position nearby within 100-200 px
        float randomX = Random.Range(1, 10) * (Random.value > 0.5f ? 1 : -1);
        float randomY = Random.Range(1, 5) * (Random.value > 0.5f ? 1 : -1);

        Vector2 randomSpot = new Vector2(core.transform.position.x + randomX, core.transform.position.y + randomY);

        // Set the new destination without wall checking
        flyingNavigateState.destination = randomSpot;
        Set(flyingNavigateState, true);
    }

    public override void Do()
    {
        core.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        if (TargetWithinVision() && TargetReachable())
        {
            if (machine.state == flyingNavigateState)
            {
                if (flyingNavigateState.isComplete)
                {
                    Set(idleFlyingState, true);
                }
            }
            else
            {
                if (machine.state.time > 4)
                {
                    GoToNextDestination();
                }
            }
        }
        else if (machine.state == idleFlyingState && machine.state.time > 5)
        {
            Debug.Log("Idle for too long, going to next destination");
            GoToNextDestination();
        }
        else
        {
            Set(idleFlyingState);
        }
    }

    private bool TargetWithinVision()
    {
        // Check if the destination is within vision range in both X and Y directions
        return Vector2.Distance(core.transform.position, flyingNavigateState.destination) < vision;
    }

    private bool TargetReachable()
    {
        // Check if the destination is vertically reachable within a reasonable range
        return Mathf.Abs(core.transform.position.y - flyingNavigateState.destination.y) < vision;
    }
}
