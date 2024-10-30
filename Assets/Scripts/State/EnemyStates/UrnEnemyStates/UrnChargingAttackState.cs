using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UrnChargingAttackState : State
{
	public FlyingChaseState flyingChaseState;
	public FlyingNavigateState flyingNavigateState;

	public AnimationClip chargingAnimation;
	public AnimationClip attackAnimation;

	public Transform target;
	public Vector3 focusPosition;

	public float chargeSpeed = 1f; // Speed at which the charge meter fills
	public float maxCharge = 2f; // Full charge level
	public float laserDuration = 2f; // Duration of the laser attack

	private float chargeMeter = 0f;
	private bool isAttacking = false;
	private float attackTimer = 4f;
	public LayerMask laserCollisionLayer; // Layers the laser can collide with
	public LineRenderer lineRenderer;

	public override void Enter()
	{
		chargeMeter = 0f;
		isAttacking = false;
		attackTimer = laserDuration;
		core.transform.Rotate(0, 0, 0);
		target = flyingChaseState.target;

		// Play charging animation and sound
		PlayChargingAnimation();
		flyingNavigateState.destination = target.position;
		Set(flyingNavigateState, true);
	}

	public override void Do()
	{
		if (!isAttacking)
		{
			//if the target is within attack range - some allowance, no need to move
			if ( Vector2.Distance(core.transform.position, target.position) > flyingChaseState.attackRange - 0.5f)
			{
				flyingNavigateState.destination = target.position;
				Set(flyingNavigateState, true);
			}
			
			ChargeAttack();
		}
		else
		{
			PerformAttack();
		}
	}

	void ChargeAttack()
	{
		// Face the target
		FaceTarget(target.position);

		// Increment charge meter
		chargeMeter += chargeSpeed * Time.deltaTime;

		// Scale animation speed based on the charge progress
		float animationSpeedMultiplier = Mathf.Lerp(1f, 5f, chargeMeter / maxCharge); // Speed up from 1x to 2.5x
		animator.speed = animationSpeedMultiplier;

		if (chargeMeter >= maxCharge)
		{
			focusPosition = target.position;
			FaceTarget(target.position); // Face the target before attacking

			// Reset charge and animation speed
			isAttacking = true;
			chargeMeter = 0f;
			animator.speed = 1f; // Reset animation speed to normal
		}
	}

	void PerformAttack()
	{
		// Transition to attack animation
		PlayAttackAnimation();

		// Freeze movement and rotation
		body.velocity = Vector2.zero;
		body.constraints = RigidbodyConstraints2D.FreezeAll;

		attackTimer -= Time.deltaTime;

		if (attackTimer >= 0)
		{
			// Perform laser attack logic here
			ShootLaser();
		}
		else
		{
			Debug.Log("Attack complete, returning to chase state.");
			// Restore movement and rotation constraints
			// Attack complete, return to chase state
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

	void OnDrawGizmos()
	{
		if (target != null)
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

	void PlayChargingAnimation()
	{
		animator.Play(chargingAnimation.name);
		// Add code to trigger charging animation and sound
		Debug.Log("Playing charging animation and sound.");
	}

	void PlayAttackAnimation()
	{
		animator.Play(attackAnimation.name);
		// Add code to trigger attack animation
		Debug.Log("Playing attack animation.");
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
			// Set the endpoint at the collision point
			lineRenderer.SetPosition(1, hit.point);
		}
		else
		{
			// If no collision, set the endpoint far in the direction of the target
			lineRenderer.SetPosition(1, (Vector2)core.transform.position + direction * 100f); // Max length of the laser
		}
		// Laser attack logic, such as enabling a laser object or firing projectiles
		Debug.Log("Shooting laser at target.");
	}

	void ExitState()
	{
		body.constraints = RigidbodyConstraints2D.None;
		lineRenderer.enabled = false; // Disable the LineRenderer
	}
}
