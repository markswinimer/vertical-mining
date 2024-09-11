using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : State
{
    public AnimationClip clip;

    public float maxXSpeed = .3f;

    public override void Enter() {
        animator.Play(clip.name);
    }
    public override void Do()
    {
        float velX = body.velocity.x;
        //play animator up to 1.5x speed based on how fast we're moving relative to max
        animator.speed = Helpers.Map(Mathf.Abs(velX), 0, maxXSpeed, 0, 1.5f, true);
        //if we leave the ground, finish
        if (!core.ground.OnGround)
        {
            isComplete = true;
        }
    }
}