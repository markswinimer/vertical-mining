using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingChaseState : State
{
    public Transform target;
    public Player player;
    
    public IdleFlyingState idleFlyingState;
    public IdleWanderState idleWanderState;
    public FlyingNavigateState flyingNavigateState;
    public UrnChargingAttackState urnChargingAttackState;

    public float vision = 10f;
    public float attackRange = 2f;
    public float idleAfterCollectTime = 1f; // Idle time after collecting an object
    public float rotationSpeed = 100f;

    public override void Enter()
    {
        flyingNavigateState.destination = target.position;
        Set(flyingNavigateState, true);
    }

    public override void Do()
    {
        CheckForTarget();
        if (state == urnChargingAttackState)
        {
            //
        }
        else
        {
            ChaseTarget();
            core.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }
    }

    void ChaseTarget()
    {
        if (state.isComplete == true && state == urnChargingAttackState)
        {
            isComplete = true;
        }
        // Check if the target is inactive
        if (!target || !target.gameObject.activeSelf)
        {
            target = null;
            isComplete = true;
            body.velocity = Vector2.zero;
        }
        else if (IsWithinReach(target.position))
        {
            // end navigation state
            Set(urnChargingAttackState, true); // Continue chasing
        }
        else if (!IsInVision(target.position))
        {
            isComplete = true;
            body.velocity = Vector2.zero;
        }
        else
        {
            core.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
            flyingNavigateState.destination = target.position;
            Set(flyingNavigateState, true); // Continue chasing
        }
    }

    void AttackTarget()
    {
        // Add any collection logic here
        Debug.Log("Collecting target: " + target.name);
    }

    void EndPursuit()
    {
        // Exit the chase state
        isComplete = true;
    }

    public bool IsInVision(Vector2 targetPos)
    {
        return Vector2.Distance(core.transform.position, targetPos) < vision;
    }

    public bool IsWithinReach(Vector2 targetPos)
    {
        return Vector2.Distance(core.transform.position, targetPos) < attackRange;
    }

    public void CheckForTarget()
    {
        Debug.Log("Checking for target");
        
        if (IsInVision(player.transform.position))
        {
            Debug.Log("Target found.");
            target = player.transform;
            Debug.Log(target);
        }
        else
        {
            Debug.Log("No target found.");
            target = null;
        }
    }

    // draw a visual representation of the vision range and also the attack range
    private void OnDrawGizmos()
    {
        if (DebugManager.Instance.DisplayEnemyGizmos == false)
        {
            return;
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, vision);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}