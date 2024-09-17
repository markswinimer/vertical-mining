using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingState : State
{
    public AnimationClip clip;
    private float _swingTimer;
    private float _playerAttackSpeed;
    private Vector3Int _swingTargetPosition;
    public bool IsSwinging { get; private set; }

    public override void Enter()
    {
        _playerAttackSpeed = Player.Instance.AttackSpeed;
        _swingTimer = _playerAttackSpeed;

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
        CheckForSwingTarget();

        // Reduce the swing timer
        _swingTimer -= Time.deltaTime;

        // If swing duration has passed, mark it as complete
        if (_swingTimer <= 0)
        {
            if (_swingTargetPosition != null)
            {
                TryHitTarget();
                _swingTimer = _playerAttackSpeed;
            }
        }
    }

    void CheckForSwingTarget()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int gridPosition = TileManager.Instance.GetTilemapWorldToCell(mousePosition);
        Debug.Log(gridPosition);
        if (TileManager.Instance.IsTileValid(gridPosition))
        {
            _swingTargetPosition = gridPosition;
        }
    }

    void TryHitTarget()
    {
        Debug.Log("Trying to hit target");
        if (_swingTargetPosition != null)
        {
            TileManager.Instance.DamageTile(_swingTargetPosition, 10f); // Example damage amount
        }
    }
}
