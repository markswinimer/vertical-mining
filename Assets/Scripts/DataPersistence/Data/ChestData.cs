using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChestData
{
	public string Id;
	public List<InventorySlot> Slots;
	
	public ChestData(string id, List<InventorySlot> slots)
	{
		Id = id;
		Slots = slots;
	}
}
