using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelBar : MonoBehaviour
{
    public Slider slider;
    public DrillMachine drillMachine;

    private void OnEnable()
    {
        drillMachine.OnFuelChanged += SetFuelLevel;
        SetMaxFuelLevel(drillMachine.DrillFuelLevelMax);
        SetFuelLevel(drillMachine.DrillFuelLevel);
    }

    private void OnDisable()
    {

        drillMachine.OnFuelChanged -= SetFuelLevel;
    }

    public void SetMaxFuelLevel(float amount)
    {
        slider.maxValue = amount;
    }

    public void SetFuelLevel(float fuelLevel)
    {
        Debug.Log("Fuel Level: " + fuelLevel);
        slider.value = fuelLevel;
    }
}
