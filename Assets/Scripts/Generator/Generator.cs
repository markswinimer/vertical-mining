using System;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public static Generator Instance { get; private set; }

    public event Action<float> OnEnergyChanged;
    public Cable cable;

    public float EnergyCapacity = 100f;
    public float Energy = 100f;
    public float EnergyDrainRate = 3f;
    public float EnergyDemand = 0.1f;

    // this will eventually compile current demand from player
    private float _cableEnergyDemand = 0f;

    private float _drainTick;

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

    private void OnEnable()
    {
        Debug.Log("Cable attached event subscribed");
        cable.OnCableAttached += PlaceEnergyDemand;
    }

    private void OnDisable()
    {
        cable.OnCableAttached -= PlaceEnergyDemand;
    }

    void Start()
    {
        //can load save here?
        _drainTick = EnergyDrainRate;
    }

    // Update is called once per frame
    void Update()
    {
        if (Energy > 0)
        {
            _drainTick -= Time.deltaTime;

            //drain energy every EnergyDrainRate seconds
            if (_drainTick <= 0)
            {
                _drainTick = EnergyDrainRate;
                DrainEnergy();
            }
        }
    }

    public void DrainEnergy()
    {
        Debug.Log(_cableEnergyDemand);
        Energy -= EnergyDemand + _cableEnergyDemand;
        OnEnergyChanged?.Invoke(Energy);
    }

    private void PlaceEnergyDemand(bool isAttached)
    {
        if (isAttached)
        {
            _cableEnergyDemand = 1f;
        }
        else 
        {
            _cableEnergyDemand = 0f;
        }
    }
}
