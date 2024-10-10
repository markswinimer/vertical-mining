using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneratorBar : MonoBehaviour
{
    public Slider slider;

    private void OnEnable()
    {
        Generator.Instance.OnEnergyChanged += SetEnergy;
    }

    private void OnDisable()
    {
        Generator.Instance.OnEnergyChanged -= SetEnergy;
    }

    public void SetMaxEnergy(float amount)
    {
        slider.maxValue = amount;
        slider.value = amount;
    }

    public void SetEnergy(float amount)
    {
        slider.value = amount;
    }
}
