using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public AnimationClip clip;

    public float jumpSpeed;
    
    public override void Enter() {
        animator.Play(clip.name);
    }
    public override void Do() {
        if (!core.ground.OnGround)
        {
            isComplete = true;
        }
    }
}