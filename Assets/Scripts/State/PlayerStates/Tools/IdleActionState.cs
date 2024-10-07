using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleActionState : State
{
    public AnimationClip clip;

    public override void Enter()
    {
        animator.Play(clip.name);
    }
}