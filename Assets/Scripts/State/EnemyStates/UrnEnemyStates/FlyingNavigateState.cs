using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingNavigateState : State
{
    public Vector2 destination;
    public float speed = 1f;
    public float threshold = 2f;
    public float _maxNavigateTime = 5f;

    public override void Enter()
    {
        isComplete = false;
    }

    public override void Do()
    {
        // Check if the enemy is close to the destination
        if (Vector2.Distance(core.transform.position, destination) < threshold  || time > _maxNavigateTime)
        {
            isComplete = true;
            body.velocity = Vector2.zero;
        }
    }

    public override void FixedDo()
    {
        Debug.DrawLine(core.transform.position, destination, Color.red); // Visual guide
        // Debug.Log(body.velocity);

        if (!isComplete)
        {
            Vector2 direction = (destination - (Vector2)core.transform.position).normalized;

            float xSpeedMultiplier = 1.5f; // Adjust as needed
            float ySpeedMultiplier = 1f;

            // In FixedDo:
            body.velocity = new Vector2(direction.x* speed * xSpeedMultiplier, direction.y* speed * ySpeedMultiplier);
        }
        else
        {
            body.velocity = Vector2.zero;
        }
    }

    public void ExitState()
    {
        isComplete = true;
    }
}
