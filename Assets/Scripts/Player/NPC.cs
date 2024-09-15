using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NPC : Core 
{
    public PatrolState patrolState;
    public CollectState collectState;
    public ChaseState chaseState;

    private Transform player;

    void Start()
    {
        player = FindFirstObjectByType<Player>().transform;
        SetupInstances();
        Set(patrolState);
    }

    void Update()
    {
        if (state.isComplete)
        {
            if (state == collectState)
            {
                Set(patrolState);
            }
            if (state == chaseState)
            {
                Set(patrolState);
            }
        }

        if (state == patrolState)
        {
            chaseState.CheckForTarget();
            if (chaseState.target != null)
            {
                Set(chaseState, true);
            } else {
                collectState.CheckForTarget();
                if (collectState.target != null)
                {
                    Set(collectState, true);
                }
            }
        }

        state.DoBranch();
    }   

    void FixedUpdate()
    {
        state.FixedDoBranch();
    }
}