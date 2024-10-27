using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Pistol : MonoBehaviour
{
    public event Action<int> OnAmmoChanged;

    public float AttackDelay = 1f;
    public float AttackDamage = 20f;
    public float ReloadSpeed = 3f;
    public int AmmoCost = 1;
    public int MaxAmmo = 3;
    public int CurrentAmmo;


    public void Start()
    {
        CurrentAmmo = MaxAmmo;
    }

    public void FillAmmo()
    {
        CurrentAmmo = MaxAmmo;
        OnAmmoChanged?.Invoke(CurrentAmmo);
    }

    public void ExpendAmmo(int amount)
    {
        CurrentAmmo -= amount;
        OnAmmoChanged?.Invoke(CurrentAmmo);
    }
}
