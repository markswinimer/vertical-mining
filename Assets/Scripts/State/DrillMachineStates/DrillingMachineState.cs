using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillingMachineState : State
{
    public AnimationClip clip;
    public AudioSource drillHitSound;

    [SerializeField] private DrillTargeter _drillTargeter;
    private List<Vector3Int> _drillPositions = new List<Vector3Int>();

    private float _drillDamage = 1f;

    public override void Enter()
    {
        _drillDamage = DrillMachine.Instance.DrillDamage;
        animator.Play(clip.name);
    }

    public override void Do()
    {
        _drillPositions = _drillTargeter.GetDrillTargetTiles();

        if (_drillPositions.Count > 0)
        {
            TryDrillTileAtPoint();
        }
    }

    void TryDrillTileAtPoint()
    {
        foreach (Vector3Int position in _drillPositions)
        {
            drillHitSound.Stop();
            drillHitSound.time = Random.Range(0.3f, .4f);
            drillHitSound.Play(0);
            TileManager.Instance.OnDamageTile(position, _drillDamage, DrillType.DrillMachine); // Example damage amount
        }
    }

}