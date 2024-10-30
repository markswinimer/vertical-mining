using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UrnChargingState : State
{
    public UrnChaseAttackState urnChaseAttackState;
    
    public AnimationClip chargingAnimation;

    public Transform target;
    public Vector3 focusPosition;

    public float chargeSpeed = 1f;
    private float _chargingTimeElapsed = 0f;
    public float _timeToFullyCharge = 2f;
    public float laserDuration = 2f;

    public override void Enter()
    {
        _chargingTimeElapsed = 0f;

        core.transform.Rotate(0, 0, 0);
        target = urnChaseAttackState.target;

        animator.Play(chargingAnimation.name);
    }

    public override void Do()
    {
        _chargingTimeElapsed += chargeSpeed * Time.deltaTime;
        target = urnChaseAttackState.target;

        if (_chargingTimeElapsed < _timeToFullyCharge)
        {
            FaceTarget(target.position);

            // Scale animation speed based on the charge progress
            float animationSpeedMultiplier = Mathf.Lerp(1f, 7f, _chargingTimeElapsed / _timeToFullyCharge); // Speed up from 1x to 2.5x
            animator.speed = animationSpeedMultiplier;
        }
        else
        {
            animator.speed = 1;
            isComplete = true;
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

    // on exit state
    public void ExitState()
    {
        body.constraints = RigidbodyConstraints2D.None;
        animator.speed = 1;
    }
}
