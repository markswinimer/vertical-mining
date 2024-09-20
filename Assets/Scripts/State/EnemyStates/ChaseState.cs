using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
	public Transform target;
	public NavigateState navigateState;
	public IdleState idleState;
	public AttackState attackState;
	public float vision = 1;
	public float attackRange = 1;

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
		if (state == navigateState || state == attackState)
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
		// check if target is dead by checking if gameobject is active
		if (!target.gameObject.activeSelf)
		{
			target = null;
			Set(idleState, true);
			body.velocity = new Vector2(0, body.velocity.y);
		}
		// could code a ram attack here
		else if (IsWithinReach(target.position))
		{
			Set(attackState);
		}
		else if (!IsInVision(target.position))
		{
			// if the target is out of vision, stop and idle
			// which will end this state
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
		// can add some custom behavior here
		// but this exits the chase state
		isComplete = true;
	}

	public override void Exit()
	{

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
		if (IsInVision(Player.Instance.gameObject.transform.position))
		{
			target = Player.Instance.transform;
			return;
		}

		target = null;
	}
}
