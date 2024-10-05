using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class ShootState : State
{

    public Transform firePoint;
    public GameObject bulletPrefab;

    public AnimationClip clip;
    private float _actionDelay;
    private float _playerAttackSpeed;
    private float _playerAttackDamage;

    public AudioSource pickaxeHitSound;

    public override void Enter()
    {
        _playerAttackSpeed = Player.Instance.AttackSpeed;
        _playerAttackDamage = Player.Instance.AttackDamage;
        _actionDelay = 0;

        // Set the animator speed to match the desired duration
        animator.Play(clip.name, 0, 0);
    }

    public override void Do()
    {
        _actionDelay -= Time.deltaTime;

        if (_actionDelay <= _playerAttackSpeed * 0.2f)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        _actionDelay = _playerAttackSpeed;
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }


    // pickaxeHitSound.Stop();
    // pickaxeHitSound.time = Random.Range(0.3f, .4f);
    // pickaxeHitSound.Play(0);
}
