using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Enemy : Core 
{
	public PatrolState PatrolState;
	public ChaseState ChaseState;

	private Transform Player;

	void Start()
	{
		Player = FindFirstObjectByType<Player>().transform;
		SetupInstances();
		Set(PatrolState);
	}

	void Update()
	{
		if (state.isComplete)
		{
			if (state == ChaseState)
			{
				Set(PatrolState);
			}
		}

		if (state == PatrolState)
		{
			ChaseState.CheckForTarget();
			if (ChaseState.target != null)
			{
				Set(ChaseState, true);
			}
		}

		state.DoBranch();
	}   

	void FixedUpdate()
	{
		state.FixedDoBranch();
	}

	void OnDrawGizmos()
	{
#if UNITY_EDITOR
		if (Application.isPlaying && state != null)
		{
			List<State> states = machine.GetActiveStateBranch();
			UnityEditor.Handles.Label(new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), "Active States: " + string.Join(", ", states));
		}
#endif
	}
}