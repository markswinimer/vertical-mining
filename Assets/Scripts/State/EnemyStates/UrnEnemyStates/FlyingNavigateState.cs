using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingNavigateState : State
{
    public Vector2 destination;
    public float speed = 1;
    public float threshold = 0.5f;

    public State animation;

    public override void Enter()
    {
        Set(animation, true);
        isComplete = false;
    }

    public override void Do()
    {
    
        if (Vector2.Distance(core.transform.position, destination) < threshold)
        {
            isComplete = true;
            body.velocity = Vector2.zero;
        }
        else
        {
            FaceDestination();
        }
    }

    public override void FixedDo()
    {
        if (!isComplete)
        {
            Vector2 direction = (destination - (Vector2)core.transform.position).normalized;
            Debug.DrawLine(core.transform.position, destination, Color.red); // Draw line to destination

            body.velocity = direction * speed;
        }
        else
        {
            body.velocity = Vector2.zero;
        }
    }

    void FaceDestination()
    {
    
        if (body.velocity.x != 0)
        {
            core.transform.localScale = new Vector3(Mathf.Sign(body.velocity.x), 1, 1);
        }
    }
}
