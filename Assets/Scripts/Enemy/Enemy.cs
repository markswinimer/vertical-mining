using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
	public EnemyState CurrentState;
	public string Name;
	public Transform Target;
	public Transform[] PatrolPoints;
	public LayerMask GroundLayer;
	
	[SerializeField]
	private int _maxHealth;
	[SerializeField]
	private int _currentHealth;
	[SerializeField]
	private float _defense;
	[SerializeField]
	private int _attackPower;
	[SerializeField]
	private float _patrolSpeed;
	[SerializeField]
	private float _chaseSpeed;
	[SerializeField]
	private float _jumpPower;
	[SerializeField]
	private float _chaseRange;
	private int _currentPatrolIndex;
	private Rigidbody2D _rigidbody;
	// Start is called before the first frame update
	private void Start()
	{
		Target = FindFirstObjectByType<Player>().transform;
		_rigidbody = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	private void Update()
	{
		switch (CurrentState)
		{
			case EnemyState.Patrolling:
				Patrol();
				break;
			case EnemyState.Chasing:
				Chase();
				break;
			case EnemyState.Jumping:
				Jump();
				break;
		}

		CheckForPlayer();
	}

	private void CheckForPlayer()
	{
		if (Vector2.Distance(transform.position, Target.position) < _chaseRange)
		{
			CurrentState = EnemyState.Chasing;
		}
		else if (CurrentState == EnemyState.Chasing)
		{
			CurrentState = EnemyState.Patrolling;
		}
	}
	
	private void Patrol()
	{
		if (PatrolPoints.Length == 0) return;

		var targetPoint = PatrolPoints[_currentPatrolIndex];
		float step = _patrolSpeed * Time.deltaTime;
		transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, step);

		if (Vector2.Distance(transform.position, targetPoint.position) < 1f)
		{
			_currentPatrolIndex = (_currentPatrolIndex + 1) % PatrolPoints.Length;
		}
	}
	
	private void Chase()
	{
		float step = _chaseSpeed * Time.deltaTime;
		transform.position = Vector2.MoveTowards(transform.position, Target.position, step);

		// Check if a jump is needed
		CheckForJump();
	}
	
	void CheckForJump()
	{
		var groundInfo = Physics2D.Raycast(transform.position, Vector2.down, 2f, GroundLayer);

		if (!groundInfo.collider)
		{
			// Jump if there's no ground ahead
			CurrentState = EnemyState.Jumping;
		}
	}

	private void Jump()
	{
		_rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpPower);
		CurrentState = EnemyState.Chasing; // Resume chasing after jumping
	}
}
