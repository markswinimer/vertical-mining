using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySlot
{
	public ItemObject Item;
	public int Amount;
	public InventorySlot(ItemObject item, int amount)
	{
		Item = item;
		Amount = amount;
	}
}
