using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
	public ItemType UpgradeItemType;
	public int Cost;
	private Inventory _drillInventory;
	public int CurrentUpgradeLevel;
	public int MaxUpgradeLevel;
	public float CostMultiplier;
	private Button _button;
	private TMP_Text _textMesh;
	public string UpgradeDescription;
	// Start is called before the first frame update
	void Start()
	{
		
	}
	
	void Awake()
	{
		_drillInventory = DrillMachine.Instance.Inventory;
		_button = GetComponent<Button>();
		_button.onClick.AddListener(TryUpgrade);
		_textMesh = GetComponentInChildren<TMP_Text>();
		_textMesh.text = FormatText();
	}

	// Update is called once per frame
	void Update()
	{
		
	}
	
	public void TryUpgrade()
	{
		Debug.Log("Upgrade button hit");
		if(CurrentUpgradeLevel < MaxUpgradeLevel && _drillInventory.TryRemoveItems(UpgradeItemType, Cost))
		{
			Unlock();
		}
	}
	
	public void Unlock()
	{
		CurrentUpgradeLevel++;
		Cost = (int)(Cost * CostMultiplier);
		_textMesh.text = FormatText();
		PerformUpgrade();
	}
	
	public virtual void PerformUpgrade()
	{
		
	}
	
	public string FormatText()
	{
		return UpgradeDescription + "<br>" + CurrentUpgradeLevel + " / " + MaxUpgradeLevel + "<br>Cost: " + Cost + " " + UpgradeItemType.ToString();
	}
}
