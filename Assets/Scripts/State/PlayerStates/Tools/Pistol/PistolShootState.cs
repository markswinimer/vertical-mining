using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class PistolShootState : State
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public GameObject fireEffect;

    [SerializeField] private Pistol _pistol;

    private int _ammoCost;

    public AudioSource pickaxeHitSound;

    public override void Enter()
    {
        _ammoCost = _pistol.AmmoCost;
    }

    public override void Do()
    {
        Shoot();
    }

    void Shoot()
    {
        _pistol.ExpendAmmo(_ammoCost);
        GameObject fireEffectVFX = Instantiate(fireEffect, firePoint.position, firePoint.rotation);
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Destroy(fireEffectVFX, 2f);
    }

}
