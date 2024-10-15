using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InvestigateState : State
{
	public Vector2 target;
	public NavigateState navigateState;
	public float InvestigationRange = 1;
	public IdleState idleState;

	// check for collectable objects
	// if you find one, go to it
	// idle for a second after collect

	public override void Enter()
	{
		Debug.Log("Enter investigate");
		navigateState.destination = target;
		Set(navigateState, true);
	}

	public override void Do()
	{
		if (Vector2.Distance(transform.position, target) > InvestigationRange)
		{
			Debug.Log("Keep investigate");
			InvestigateLastSeen();
		}
		else
		{
			Debug.Log("Exit Investigate");
			EndPursuit();
		}
	}

	void InvestigateLastSeen()
	{
		
	}

	void EndPursuit()
	{
		Set(idleState, true);
		body.velocity = new Vector2(0, body.velocity.y);
		isComplete = true;
	}

	public override void Exit()
	{

	}
}
