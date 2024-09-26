using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDataPersistence
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

	public void LoadData(GameData data)
	{
		Inventory.Container = data.PlayerData.Inventory;
		Health = data.PlayerData.Health;
		transform.position = data.PlayerData.Position;
	}

	public void SaveData(GameData data)
	{
		data.PlayerData.Inventory = Inventory.Container;
		data.PlayerData.Health = Health;
		data.PlayerData.Position = transform.position;
	}
}
