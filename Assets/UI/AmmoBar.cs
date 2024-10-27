using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoBar : MonoBehaviour
{
    public Slider slider;
    public Inventory inventory;

    private void OnEnable()
    {
        // Subscribe to the OnPlayerDamaged event
        inventory.OnAmmoChanged += SetAmmo;
    }

    private void OnDisable()
    {
        // Unsubscribe from the event to prevent memory leaks
        inventory.OnAmmoChanged -= SetAmmo;
    }

    public void SetMaxAmmo(int ammo)
    {
        slider.maxValue = ammo;
        slider.value = ammo;
    }

    public void SetAmmo(int ammo)
    {
        slider.value = ammo;
    }
}
