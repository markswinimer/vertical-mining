using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	// singleton instance
	public static Player Instance { get; private set; }
	
	public Inventory Inventory;
	public float AttackSpeed  = .5f;
	public float AttackDamage = 20f;
	public int Health = 5;

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
	
	public void DealDamage(int damage)
	{
		Health -= damage;
	}
	
	private void OnApplicationQuit() {
		Inventory.Container.Clear();
	}
}
