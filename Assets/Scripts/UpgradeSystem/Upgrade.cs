using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
	public ItemType UpgradeItemType;
	public int Cost;
	private Inventory _drillInventory;
	public int CurrentUpgradeLevel;
	public int MaxUpgradeLevel;
	public float CostMultiplier;
	// Start is called before the first frame update
	void Start()
	{
		
	}
	
	void Awake()
	{
		_drillInventory = DrillMachine.Instance.Inventory;
	}

	// Update is called once per frame
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.R))
		{
			Debug.Log("R Hit");
			TryUpgrade();
		}
	}
	
	public void TryUpgrade()
	{
		if(CurrentUpgradeLevel < MaxUpgradeLevel && _drillInventory.TryRemoveItems(UpgradeItemType, Cost))
		{
			Unlock();
		}
	}
	
	public void Unlock()
	{
		CurrentUpgradeLevel++;
		Cost = (int)(Cost * CostMultiplier);
		PerformUpgrade();
	}
	
	public virtual void PerformUpgrade()
	{
		
	}
}
