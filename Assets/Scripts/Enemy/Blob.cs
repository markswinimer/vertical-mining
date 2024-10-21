using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Blob : Enemy 
{
	public CeilingState CeilingState;
	public CeilingFallState CeilingFallState;
	public AirState FallState;
	public LandState LandState;
	public IdleState IdleState;

	void Update()
	{
		if (state.isComplete)
		{
			if (state == ChaseState || state == InvestigateState)
			{
				Set(IdleState);
			}
			else if (state == CeilingFallState)
			{
				Set(FallState, true);
			}
			else if (state == FallState)
			{
				Set(LandState, true);
			}
			else if (state == LandState)
			{
				Set(ChaseState, true);
			}
		}

		if (state == IdleState || state == InvestigateState || state == CeilingState)
		{
			ChaseState.CheckForTarget();
			if (ChaseState.target != null)
			{
				if(state == CeilingState)
				{
					Set(CeilingFallState, true);
				}
				else
				{
					Set(ChaseState, true);
				}
			}
		}

		state.DoBranch();
	}

	void OnDrawGizmos()
	{
#if UNITY_EDITOR
			if (Application.isPlaying && state != null)
			{
				List<State> states = machine.GetActiveStateBranch();
				UnityEditor.Handles.Label(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), "Active States: " + string.Join(", ", states));
			}
#endif
	}
}