using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
	public List<InventorySlot> Inventory = new List<InventorySlot>();
	public Vector3 Position;
	public int Health;
}
