using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Object", menuName = "Inventory System/Items/Default")]
public class ItemObject : ScriptableObject
{
	public GameObject Prefab;
	public ItemType ItemType;
	[TextArea(15, 20)]
	public string Description;
	public int MaxStackSize;
}
