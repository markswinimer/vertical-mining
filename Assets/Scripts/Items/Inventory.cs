using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory/Default")]
public class Inventory : ScriptableObject
{
	public List<InventorySlot> Container = new List<InventorySlot>();
	
	public void AddItem(ItemObject item, int amount)
	{
		//get all stacks of specific item
		var slots = Container.Where(it => it.Item.ItemType == item.ItemType && it.Amount < it.Item.MaxStackSize);
		
		//add amount to stacks, track amount to add remaining
		foreach(var slot in slots)
		{
			var slotSizeRemaining = slot.Item.MaxStackSize - slot.Amount;
			var amountToAdd = Mathf.Min(slotSizeRemaining, amount);
			slot.Amount += amountToAdd;
			amount -= amountToAdd;
			if(amount <= 0)
			{
				break;
			}
		}
	
		//if still has more remaining, add new slots with rest
		if (amount <= 0) return;

		while(amount > 0)
		{
			var amountToAdd = Mathf.Min(item.MaxStackSize, amount);
			Container.Add(new InventorySlot(item, amountToAdd));
			amount -= amountToAdd;
		}
	}

	public int GetItemCountByName(ItemType itemType)
	{
		int count = Container.Where(it => it.Item.ItemType == itemType).Sum(it => it.Amount);
		return count;
	}
}
