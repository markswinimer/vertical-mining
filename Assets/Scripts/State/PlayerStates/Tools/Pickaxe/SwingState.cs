using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class SwingState : State
{
    public AnimationClip clip;
    private float _swingTimer;
    private float _animationSpeed;
    private float _playerAttackSpeed;
    private float _playerAttackDamage;
    private Vector3Int _swingTargetPosition;
    private bool _hasSwingTarget;
    public bool IsSwinging { get; private set; }

    public AudioSource pickaxeHitSound;

    private bool _swingWillResolve;

    public override void Enter()
    {
        _playerAttackSpeed = Player.Instance.AttackSpeed;
        _playerAttackDamage = Player.Instance.AttackDamage;
        _swingTimer = _playerAttackSpeed;

        IsSwinging = true;
        _swingWillResolve = false;

        // Calculate the speed needed for the animation to match the desired duration
        float animationSpeed = clip.length / _swingTimer;
        // Set the animator speed to match the desired duration
        animator.speed = animationSpeed;
        animator.Play(clip.name, 0, 0);
    }

    public override void Do()
    {
        if (!_swingWillResolve)
        {
            Swing();
        }
    }

    void Swing()
    {
        CheckForSwingTarget();
        // Reduce the swing timer
        _swingTimer -= Time.deltaTime;

        // If swing duration has passed, mark it as complete
        if (_swingTimer <= _playerAttackSpeed * 0.2f)
        {
            if (_swingTargetPosition != null)
            {
                TryHitTarget();
                // swing animation has backswing, so we need to wait a bit before resetting the swing
                // this will sink up the audio and visual effects
                _swingWillResolve = true;
                // animation will stop after finishing this loop
                StartCoroutine(FinishCurrentAnimationLoop());
            }
        }
    }

    IEnumerator FinishCurrentAnimationLoop()
    {
        // Get the current state of the animation
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // Calculate the remaining time, factoring in the animation speed
        float remainingTime = (clip.length - stateInfo.normalizedTime * clip.length) / animator.speed;

        yield return new WaitForSeconds(remainingTime);

        IsSwinging = false;
        isComplete = true;
    }

    void CheckForSwingTarget()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int gridPosition = TileManager.Instance.GetTilemapWorldToCell(mousePosition);
        if (TileManager.Instance.IsTileValid(gridPosition))
        {
            _swingTargetPosition = gridPosition;
            _hasSwingTarget = true;
        }
        else
        {
            _hasSwingTarget = false;
        }
    }

    void TryHitTarget()
    {
        if (_swingTargetPosition != null && _hasSwingTarget)
        {
            pickaxeHitSound.Stop();
            pickaxeHitSound.time = Random.Range(0.3f, .4f);
            pickaxeHitSound.Play(0);
            TileManager.Instance.OnDamageTile(_swingTargetPosition, _playerAttackDamage, DrillType.Player); // Example damage amount
        }
    }
}
