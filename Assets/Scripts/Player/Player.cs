using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Player : MonoBehaviour, IDataPersistence
{
	// singleton instance
	public static Player Instance { get; private set; }

	public event Action<int> OnHealthChanged;

	public Inventory Inventory;
	public float AttackSpeed  = .5f;
	public float AttackDamage = 20f;
	public int MaxAmmo = 100;
	public int Ammo;
	public int Health = 5;
	public DistanceJoint2D DistanceJoint;
	public Cable cable;

	public GameObject lightBasic;
	public GameObject lightCable;

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
		if(cable != null) cable.OnCableAttached += UpdateDistanceJointOnCableSwitch;
	}

	private void OnEnable()
	{
		Debug.Log("Cable attached event subscribed");
		if(cable != null) cable.OnCableAttached += ModifyLightSource;
	}

	private void OnDisable()
	{
		if(cable != null) cable.OnCableAttached -= ModifyLightSource;
	}

	public void Start()
	{
		if(cable != null) ModifyLightSource(cable.IsAttachedToPlayer);
		Ammo = Inventory.GetItemCountByName(ItemType.Ore);
		OnHealthChanged?.Invoke(Health);
	}

	public void DealDamage(int damage)
	{
		Health -= damage;
		OnHealthChanged?.Invoke(Health);
	}

	public void LoadData(GameData data)
	{
		Inventory.Container = data.PlayerData.Inventory;
		Health = data.PlayerData.Health;
		// Ammo = data.PlayerData.Ammo;
		transform.position = data.PlayerData.Position;
	}

	public void SaveData(GameData data)
	{
		data.PlayerData.Inventory = Inventory.Container;
		data.PlayerData.Health = Health;
		data.PlayerData.Position = transform.position;
	}
	
	public void PauseGravity()
	{
		GetComponent<Rigidbody2D>().gravityScale = 0;
	}
	
	public void SetGravity()
	{
		GetComponent<Rigidbody2D>().gravityScale = 4;
	}

	public void ModifyLightSource(bool isAttached)
	{
		if (isAttached)
		{
			lightBasic.SetActive(false);
			lightCable.SetActive(true);
		}
		else
		{
			lightBasic.SetActive(true);
			lightCable.SetActive(false);
		}
	}
	private void UpdateDistanceJointOnCableSwitch(bool isAttached)
	{
		Debug.Log("Update distance joint = " + isAttached.ToString());
		if (isAttached)
		{
			DistanceJoint.enabled = true;
		}
		else 
		{
			DistanceJoint.enabled = false;
		}
	}
}
