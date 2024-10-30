using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UrnChaseAttackState : State
{
    public Transform target;
    public Player player;

    public UrnChargingState urnChargingState;
    public UrnFiringState urnFiringState;
    public IdleFlyingState idleFlyingState;

    private bool _isFiring = false;

    private float _cooldownTime = 2f;
    private float _elapsedCooldownTime = 0f;

    public float vision = 10f;
    public float attackRange = 2f;
    public float rotationSpeed = 100f;

    public Vector2 destination;
    public float speed = 1f;
    public float threshold = 2f;

    public override void Enter()
    {
    }

    public override void Do()
    {
        CheckForTarget();

        if (!target || !target.gameObject.activeSelf)
        {
            target = null;
            isComplete = true;
            return;
        }

        if (machine.state == urnFiringState && urnFiringState.isComplete)
        {
            _isFiring = false;
            _elapsedCooldownTime = _cooldownTime;
            Set(idleFlyingState, true);
        }
        if (machine.state == idleFlyingState && _elapsedCooldownTime > 0)
        {
            core.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
            _elapsedCooldownTime -= Time.deltaTime;
            Set(idleFlyingState);
        }
        else if (_isFiring == true)
        {
            Set(urnFiringState);
        }
        else if (machine.state == urnChargingState && urnChargingState.isComplete)
        {
            _isFiring = true;
            Set(urnFiringState, true);
        }
        else
        {
            FaceTarget(target.position);

            if (IsWithinReach(target.position))
            {
                Set(urnChargingState);
            }
            else if (!IsInVision(target.position))
            {
                isComplete = true;
            }
        }
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
        if (IsInVision(player.transform.position))
        {
            target = player.transform;
        }
        else
        {
            target = null;
        }
    }

    public override void FixedDo()
    {
        if (state != urnFiringState && target != null)
        {
            // Check if the enemy is outside the attack range
            if (Vector2.Distance(core.transform.position, target.position) > attackRange - 0.5f)
            {
                Debug.DrawLine(core.transform.position, target.position, Color.red); // Visual guide

                // Calculate direction towards the target's current position
                Vector2 direction = (target.position - core.transform.position).normalized;

                // Adjust speed multipliers if needed
                float xSpeedMultiplier = 1.5f;
                float ySpeedMultiplier = 1f;

                // Set the velocity to move toward the target
                body.velocity = new Vector2(direction.x * speed * xSpeedMultiplier, direction.y * speed * ySpeedMultiplier);
            }
            else
            {
                // Stop moving when within attack range
                body.velocity = Vector2.zero;
            }
        }
    }

    void FaceTarget(Vector3 targetPosition)
    {
        // Calculate the direction vector from the enemy to the target
        Vector3 direction = (targetPosition - core.transform.position).normalized;

        // Calculate the angle in degrees to rotate towards the target
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Apply the calculated angle to the z-axis rotation to face the target
        core.transform.rotation = Quaternion.Euler(0, 0, angle);
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

    public void ExitState()
    {
        body.constraints = RigidbodyConstraints2D.None;
        animator.speed = 1;
    }
}