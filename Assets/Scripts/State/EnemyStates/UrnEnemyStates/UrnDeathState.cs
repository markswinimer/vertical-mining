using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UrnDeathState : State
{
    public AnimationClip clip;
    [SerializeField] private SpriteRenderer spriteRenderer;

    public override void Enter()
    {
        core.transform.Rotate(0, 0, 0);
        // add set gravity factorto 2
        body.gravityScale = 2; //
        animator.Play(clip.name);
    }

    public override void Do()
    {
        // wait a few seconds before completing using ienumerator
        //reduce alpha every frame
        spriteRenderer.color = new Color(1, 1, 1, 1 - time / 2);
        if (time > 2)
        {
            //fade out sprite on death
            isComplete = true;
        }
    }
}