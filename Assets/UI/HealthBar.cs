using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    private void OnEnable()
    {
        Player.Instance.OnHealthChanged += SetHealth;
    }

    private void OnDisable()
    {
        Player.Instance.OnHealthChanged -= SetHealth;
    }

    public void SetMaxHealth(int amount)
    {
        slider.maxValue = amount;
        slider.value = amount;
    }

    public void SetHealth(int amount)
    {
        slider.value = amount;
    }
}
