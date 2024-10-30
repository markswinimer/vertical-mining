using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UrnFiringState : State
{
    public AnimationClip attackAnimation;
    public UrnChaseAttackState urnChaseAttackState;
    public Transform target;
    public Vector3 focusPosition;

    public GameObject impactEffect;

    public float _attackDuration = 2f; // Duration of the laser attack
    private float _firingTimeElapsed;

    public LayerMask laserCollisionLayer; // Layers the laser can collide with
    public LineRenderer lineRenderer;

    public override void Enter()
    {
        _firingTimeElapsed = 0f;
        focusPosition = urnChaseAttackState.target.position;
        FaceTarget(focusPosition);

        // Freeze movement and rotation
        body.velocity = Vector2.zero;
        body.constraints = RigidbodyConstraints2D.FreezeAll;
        
        animator.speed = 1;
        animator.Play(attackAnimation.name);
    }

    public override void Do()
    {
        _firingTimeElapsed += Time.deltaTime;

        if (_firingTimeElapsed < _attackDuration)
        {
            ShootLaser();
        }
        else
        {
            body.constraints = RigidbodyConstraints2D.None;
            lineRenderer.enabled = false;

            isComplete = true;
        }
    }

    void ShootLaser()
    {
        // Enable the LineRenderer for the laser
        lineRenderer.enabled = true;

        // Set the start point of the laser at the enemy's position
        lineRenderer.SetPosition(0, core.transform.position);

        // Cast a ray towards the target position
        Vector2 direction = (focusPosition - core.transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(core.transform.position, direction, Mathf.Infinity, laserCollisionLayer);

        if (hit.collider != null)
        {

            IDamageable damageable = hit.collider.GetComponent<IDamageable>();

            if (damageable != null)
            {
                // get damageable transform position
                Transform damageableTransform = hit.collider.GetComponent<Transform>();
                GameObject impactEffectVFX = Instantiate(impactEffect, damageableTransform.position, transform.rotation);
                Destroy(impactEffectVFX, 2f);
                
                if (damageable.Invincible == false)
                {
                    damageable.TakeDamage(3);
                }
            }
            // Set the endpoint at the collision point
            lineRenderer.SetPosition(1, hit.point);
        }
        else
        {
            // If no collision, set the endpoint far in the direction of the target
            lineRenderer.SetPosition(1, (Vector2)core.transform.position + direction * 100f); // Max length of the laser
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

    void OnDrawGizmos()
    {
        if (target != null && DebugManager.Instance.DisplayEnemyGizmos)
        {
            // Calculate the direction vector from the enemy to the target
            Vector3 direction = (target.position - core.transform.position).normalized;

            // Set the Gizmos color for visibility
            Gizmos.color = Color.red;

            // Draw a line from the enemy's position to the target
            Gizmos.DrawLine(core.transform.position, core.transform.position + direction * 2f); // Adjust the length as needed

            // Draw an arrowhead to indicate direction
            Vector3 arrowHead1 = Quaternion.Euler(0, 0, 30) * direction * 0.5f; // First side of the arrowhead
            Vector3 arrowHead2 = Quaternion.Euler(0, 0, -30) * direction * 0.5f; // Second side of the arrowhead

            Gizmos.DrawLine(core.transform.position + direction * 2f, core.transform.position + direction * 1.5f + arrowHead1);
            Gizmos.DrawLine(core.transform.position + direction * 2f, core.transform.position + direction * 1.5f + arrowHead2);
        }
    }
    // on exit state
    public void ExitState()
    {
        body.constraints = RigidbodyConstraints2D.None;
        lineRenderer.enabled = false;
        animator.speed = 1;
    }
}
