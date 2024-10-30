using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UrnDeathState : State
{
    public AnimationClip clip;

    public override void Enter()
    {
        core.transform.Rotate(0, 0, 0);
        // add set gravity factorto 2
        body.gravityScale = 2; //
        animator.Play(clip.name);
    }

    public override void Do()
    {
        
    }
}