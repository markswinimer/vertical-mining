using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillMachine : MonoBehaviour
{
    // singleton instance
    public static DrillMachine Instance { get; private set; }

    public float DrillFuelLevelMax = 100f;
    public float DrillFuelLevel = 0f;

    public float _drillFuelDemand = 0.5f;
    
    private float _fuelUpdateInterval = .2f;
    private float _taxFuelInterval = 1f;
    private float _taxFuelIntervalMax = 1f;

    public event Action<float> OnFuelChanged;

    public Inventory Inventory;
    public bool isPowered;
    public float DrillDamage = 10f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        if (DrillHasFuel())
        {
            isPowered = true;
        }
    }

    public void Update()
    {
        _fuelUpdateInterval -= Time.deltaTime;
        _taxFuelInterval -= Time.deltaTime;

        if (_fuelUpdateInterval <= 0 && DrillFuelLevel <= DrillFuelLevelMax)
        {
            ConvertOreToFuel();
        }

        if (DrillFuelLevel > 0)
        {
            isPowered = true;
        }
        else
        {
            isPowered = false;
        }

        if (isPowered && _taxFuelInterval <= 0)
        {
            TaxFuelAmount();
            _taxFuelInterval = _taxFuelIntervalMax;
        }
    }

    private bool DrillHasFuel()
    {
        return DrillFuelLevel > 0;
    }

    private void TaxFuelAmount()
    {
        DrillFuelLevel -= _drillFuelDemand;
        OnFuelChanged?.Invoke(DrillFuelLevel);
    }

    private void ConvertOreToFuel()
    {
        ItemObject item = Inventory.TryRemoveAndGetItem(ItemType.Ore);

        if (item != null)
        {
            DrillFuelLevel += 3f;
        }
    }
}
