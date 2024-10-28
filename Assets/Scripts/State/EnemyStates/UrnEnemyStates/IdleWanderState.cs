using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleWanderState : State
{
    public FlyingNavigateState flyingNavigateState;
    public IdleFlyingState idleFlyingState;

    [SerializeField] private UrnEnemy _urnEnemy;

    private Vector3 _spawnPosition;
    public float vision = 10;

    private Vector2 _destination;

    private float minMoveDistance = 1f;  // 100 pixels
    private float maxMoveDistance = 10f;  // 300 pixels

    private float _timeToIdle = 10f;
    private float _timeIdle;
    
    public LayerMask groundLayer; // LayerMask to check for walls

    public override void Enter()
    {
        _timeIdle = 0f;
        _spawnPosition = _urnEnemy._spawnPosition;
    }

    void TryGetDestination()
    {        
        Vector2 randomSpot;
        int maxAttempts = 10; // Limit attempts to avoid infinite loops
        int attempts = 0;

        do
        {
            // Calculate a random spot nearby within the specified range
            float randomX = Random.Range(-maxMoveDistance, maxMoveDistance);
            float randomY = Random.Range(-maxMoveDistance, maxMoveDistance);

            randomSpot = new Vector2(_spawnPosition.x + randomX, _spawnPosition.y + randomY);
            Debug.Log(randomSpot + " the spot");
            Debug.Log(core.transform.position + " current");
            attempts++;

        } while (Physics2D.OverlapPoint(randomSpot, groundLayer) && attempts < maxAttempts);

        // If a valid point is found within the max attempts, set it as the destination
        if (attempts < maxAttempts)
        {
            _destination = randomSpot;
        }
    }

    public override void Do()
    {
        _timeIdle -= Time.deltaTime;

        if (TargetWithinVision() && TargetReachable())
        {
            if (machine.state == flyingNavigateState)
            {
                if (flyingNavigateState.isComplete)
                {
                    Debug.Log("set urn Idle");
                    Set(idleFlyingState);
                }
                else if (_timeIdle <= 0)
                {
                    _timeIdle = _timeToIdle;
                    TryGetDestination();
                }
            }
        }
        else
        {
            _timeIdle = _timeToIdle;
            TryGetDestination();
        }
    }

    private bool TargetWithinVision()
    {
        return Vector2.Distance(_urnEnemy.transform.position, _destination) < vision;
    }

    private bool TargetReachable()
    {
        return Mathf.Abs(_urnEnemy.transform.position.y - _destination.y) < 2;
    }
}
