using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingState : State
{
    public AnimationClip clip;
    private bool _startedSwinging = false;
    private float _swingTimer;
    public bool IsSwinging { get; private set; }

    public override void Enter()
    {
        _swingTimer = Player.Instance.AttackSpeed;

        // Calculate the speed needed for the animation to match the desired duration
        float animationLength = clip.length;
        float animationSpeed = animationLength / _swingTimer;

        // Set the animator speed to match the desired duration
        animator.speed = animationSpeed;

        animator.Play(clip.name, 0, 0);
        IsSwinging = true;
    }

    public override void Do()
    {
        if (!IsSwinging) {
            isComplete = true;
            // set the animator back to normal
            return;
        }

        // Reduce the swing timer
        _swingTimer -= Time.deltaTime;
        // If swing duration has passed, mark it as complete
        if (_swingTimer <= 0)
        {
            IsSwinging = false;
        }
    }
}
