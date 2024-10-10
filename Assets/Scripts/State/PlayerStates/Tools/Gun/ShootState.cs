using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class ShootState : State
{
    public Inventory inventory;

    public Transform firePoint;
    public GameObject bulletPrefab;
    public GameObject fireEffect;

    private float _actionDelay;
    private float _playerAttackSpeed;
    private float _playerAttackDamage;

    [SerializeField] private ItemObject _ammoType;
    [SerializeField] private int _ammoCost  ;

    public AudioSource pickaxeHitSound;

    public override void Enter()
    {
        _playerAttackSpeed = Player.Instance.AttackSpeed;
        _playerAttackDamage = Player.Instance.AttackDamage;
        _actionDelay = 0;
    }

    public override void Do()
    {
        _actionDelay -= Time.deltaTime;

        if (_actionDelay <= _playerAttackSpeed * 0.2f)
        {
            bool hasAmmo = CheckAmmo();

            if (hasAmmo)
            {
                HandleAmmo();
                Shoot();
            }
        }
    }

    void Shoot()
    {
        _actionDelay = _playerAttackSpeed;
        GameObject fireEffectVFX = Instantiate(fireEffect, firePoint.position, firePoint.rotation);
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Destroy(fireEffectVFX, 2f);
    }

    private bool CheckAmmo()
    {
        int ammo = inventory.GetItemCountByName(ItemType.Ore);
        return ammo >= _ammoCost;
    }

    private void HandleAmmo()
    {
        inventory.RemoveItem(_ammoType, _ammoCost);
    }
    // pickaxeHitSound.Stop();
    // pickaxeHitSound.time = Random.Range(0.3f, .4f);
    // pickaxeHitSound.Play(0);
}
