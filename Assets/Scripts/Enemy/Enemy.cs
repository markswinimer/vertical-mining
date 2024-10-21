using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Enemy : Core 
{
	public PatrolState PatrolState;
	public ChaseState ChaseState;
	public InvestigateState InvestigateState;
	public State StartingState;

	private Transform Player;

	private float _health = 100;

	void Start()
	{
		Player = FindFirstObjectByType<Player>().transform;
		SetupInstances();
		Set(StartingState);
	}

	void Update()
	{
		if (state.isComplete)
		{
			if (state == ChaseState || state == InvestigateState)
			{
				Set(PatrolState);
			}
		}

		if (state == PatrolState || state == InvestigateState)
		{
			ChaseState.CheckForTarget();
			if (ChaseState.target != null)
			{
				Set(ChaseState, true);
			}
		}

		state.DoBranch();
	}

	public void TakeDamage(float damage)
	{
		_health -= damage;
		if (_health <= 0)
		{
			HandleDeath();
		}
	}

	void HandleDeath()
	{	
		// Instantiate(deathEffect, transform.position, Quaternion.identity);
		Destroy(gameObject);
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