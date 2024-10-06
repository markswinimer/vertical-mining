using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObtainableItem : MonoBehaviour
{
	public ItemObject Item;
	public int Amount = 1;
	
	private void OnTriggerEnter2D(Collider2D other) {
		
		if(other.gameObject.GetComponent<Player>() != null)
		{
			Player.Instance.Inventory.AddItem(Item, Amount);
			Destroy(gameObject);
		}
	}
}
