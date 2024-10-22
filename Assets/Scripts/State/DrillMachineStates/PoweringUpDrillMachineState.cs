using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoweringUpDrillMachineState : State
{
    public AnimationClip clip;
    
    public float drillPowerUpSpeed = 3f;
    private float _lapsedTime = 0f;

    public override void Enter()
    {
        animator.Play(clip.name);
    }

    public override void Do()
    {
        _lapsedTime += Time.deltaTime;

        if (_lapsedTime >= drillPowerUpSpeed)
        {
            isComplete = true;
        }
    }
}