using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory/Default")]
public class Inventory : ScriptableObject
{
	public List<InventorySlot> Container = new List<InventorySlot>();
	
	public event Action<int> OnAmmoChanged;
	
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

			PushInventoryUpdate(item.ItemType);

			if (amount <= 0)
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
	
	public void RemoveItem(ItemObject item, int amount)
	{
		//get all stacks of specific item
		var slots = Container.Where(it => it.Item.ItemType == item.ItemType);
		//add amount to stacks, track amount to add remaining
		foreach(var slot in slots)
		{
			var amountToRemove = Mathf.Min(slot.Amount, amount);
			slot.Amount -= amountToRemove;
			amount -= amountToRemove;

			if (slot.Amount <= 0)
			{
				Container.Remove(Container.First(it => it.Item.ItemType == item.ItemType));
				break;
			}
			if (amount <= 0)
			{
				break;
			}
		}
		PushInventoryUpdate(item.ItemType);
	}

	public int GetItemCountByName(ItemType itemType)
	{
		var slots = Container.Where(it => it.Item.ItemType == itemType);
		return slots.Sum(it => it.Amount);
	}

	private void PushInventoryUpdate(ItemType itemType)
	{
		switch (itemType)
		{
			case ItemType.Ore:
				int amount = GetItemCountByName(ItemType.Ore);
				OnAmmoChanged?.Invoke(amount);
				break;
			default:
				break;
		}
	}

	public ItemObject TryRemoveAndGetItem(ItemType itemType)
	{
		var slots = Container.Where(it => it.Item.ItemType == itemType);
		
		if (slots.Count() == 0)
		{
			return null;
		}
		
		var slot = slots.First();
		
		RemoveItem(slot.Item, 1);
		
		return slot.Item;
	}

	public int GetTotalInventoryCount()
	{
		int total = 0;
		foreach (var slot in Container)
		{
			total += slot.Amount;
		}
		return total;
	}
}
