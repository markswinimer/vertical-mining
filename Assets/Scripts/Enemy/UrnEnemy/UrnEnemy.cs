using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class UrnEnemy : Core
{
	public IdleFlyingState idleFlyingState;
	public IdleWanderState idleWanderState;
	public FlyingChaseState flyingChaseState;
	public FlyingNavigateState flyingNavigateState;
	public UrnChargingState urnChargingState;
	public UrnAttackingState urnAttackingState;

    public State StartingState;

    public Vector3 _spawnPosition;
    private Transform Player;

    public float _health = 100;

    private Rigidbody2D _rb;

    void Start()
    {
        SetupInstances();

        Player = FindFirstObjectByType<Player>().transform;

        _rb = GetComponent<Rigidbody2D>();
        _rb.velocity = Vector2.zero;
        _spawnPosition = transform.position;
        
        Set(StartingState);
    }

    void Update()
    {
        if (state.isComplete)
        {
            if (state == idleFlyingState)
            {
                Set(idleWanderState);
            }
            else if (state == flyingChaseState)
            {
                Set(idleFlyingState, true);
            }
        }

        // if (state == PatrolState || state == InvestigateState)
        // {
        //     ChaseState.CheckForTarget();
        //     if (ChaseState.target != null)
        //     {
        //         Set(ChaseState, true);
        //     }
        // }

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