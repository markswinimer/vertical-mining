using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UIElements;

public class DrillingState : State
{
    public AnimationClip clip;

    public Transform firePoint;

    private float _actionDelay;
    private float _playerAttackSpeed;
    private float _playerAttackDamage;

    private Vector3Int _targetPosition;
    private bool _hasTarget;

    public Transform drillPointA;
    public Transform drillPointB;
    public Transform drillTipTop;
    public Transform drillTipBottom;

    public Transform drillPoint;
    public float drillRange = .5f;
    public LayerMask tilemapLayer;
    public AudioSource pickaxeHitSound;
    public Vector3 drillPosition;

    public override void Enter()
    {
        _playerAttackSpeed = Player.Instance.AttackSpeed;
        _playerAttackDamage = Player.Instance.AttackDamage;
        _actionDelay = 0;

        // Calculate the speed needed for the animation to match the desired duration
        float animationSpeed = clip.length / _playerAttackSpeed;
        // Set the animator speed to match the desired duration
        animator.speed = animationSpeed;
        animator.Play(clip.name, 0, 0);
    }

    public override void Do()
    {
        drillPosition = drillPoint.position;
        _actionDelay += Time.deltaTime;
        TriggerDrill();
    }

    void TriggerDrill()
    {
        CheckForTargets();

        if (_hasTarget)
        {
            HandleDrillEffect();

            if (_actionDelay >= _playerAttackSpeed * 1.2f)
            {
                _actionDelay = 0;
                TryHitTarget();
            }
        }
    }

    private void HandleDrillEffect()
    {
        // Handle drill effect here
    }

    void CheckForTargets()
    {
        _hasTarget = false; // Reset at the start
        _targetPosition = default(Vector3Int); // Reset to default value

        Vector3Int gridPositionTop = TileManager.Instance.GetTilemapWorldToCell(drillTipTop.position);
        Vector3Int gridPositionBottom = TileManager.Instance.GetTilemapWorldToCell(drillTipTop.position);

        if ( TileManager.Instance.IsTileValid(gridPositionTop) && TileManager.Instance.IsTileValid(gridPositionBottom) )
        {
            Vector3Int gridPositionA = TileManager.Instance.GetTilemapWorldToCell(drillTipTop.position);
            if ( TileManager.Instance.IsTileValid(gridPositionTop) )
            {
                _targetPosition = gridPositionA;
                _hasTarget = true;
                return;
            }
        }
    }

    void TryHitTarget()
    {
        if (_hasTarget)
        {
            pickaxeHitSound.Stop();
            pickaxeHitSound.time = Random.Range(0.3f, .4f);
            pickaxeHitSound.Play(0);
            TileManager.Instance.OnDamageTile(_targetPosition, _playerAttackDamage); // Example damage amount
        }
    }
}
