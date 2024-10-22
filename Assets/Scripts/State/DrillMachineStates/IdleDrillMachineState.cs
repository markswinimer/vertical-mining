using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleDrillMachineState : State
{
    public AnimationClip clip;

    public override void Enter()
    {
        animator.Play(clip.name);
    }

    // public override void Do()
    // {
    //     //seek the animator to the frame based on our y velocity
    //     // float time = Helpers.Map(body.velocity.y, jumpSpeed, -jumpSpeed, 0, 1, true);
    //     float time = 1f;
    //     animator.Play(clip.name, 0, time);
    //     animator.speed = 0;

    //     if (core.ground.OnGround)
    //     {
    //         isComplete = true;
    //     }
    // }
}