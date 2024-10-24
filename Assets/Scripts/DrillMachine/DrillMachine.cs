using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillMachine : MonoBehaviour
{
    // singleton instance
    public static DrillMachine Instance { get; private set; }

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
        isPowered = true;
    }

    public void Update()
    {

    }
}
