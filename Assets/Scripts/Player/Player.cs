using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Player : MonoBehaviour, IDataPersistence, IDamageable
{
	// singleton instance
	public static Player Instance { get; private set; }

	public event Action<int> OnHealthChanged;

	[SerializeField] private SpriteRenderer spriteRenderer;

	public Inventory Inventory;
	public float AttackSpeed  = .5f;
	public float AttackDamage = 20f;
	public int MaxAmmo = 100;
	public int Ammo;
	public int Health = 50;
	public int MaxHealth = 50;
	public DistanceJoint2D DistanceJoint;
	public Cable cable;

	public GameObject lightBasic;
	public GameObject lightCable;

	private bool _willDie = false;

	public bool Invincible { get; set; }
	private float _invincibleTime = .6f;
	private float _timeSpentInvincible { get; set; }

	private Rigidbody2D _rb;
	private Color originalColor;

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
		_rb = GetComponent<Rigidbody2D>();
		Invincible = false;
		if(cable != null) ModifyLightSource(cable.IsAttachedToPlayer);
		Ammo = Inventory.GetItemCountByName(ItemType.Ore);
		OnHealthChanged?.Invoke(Health);

		if (spriteRenderer != null)
		{
			originalColor = spriteRenderer.color;
		}
	}

	public void Update()
	{
		if (Invincible)
		{
			_timeSpentInvincible += Time.deltaTime;

			// modify the alpha of the sprite renderer to make the player flash
			if (_timeSpentInvincible % 0.1f < 0.05f)
			{
				spriteRenderer.color = new Color(1, 1, 1, 0);
			}
			else
			{
				spriteRenderer.color = originalColor;
			}

			if (_timeSpentInvincible < _invincibleTime)
			{
				Invincible = false;
				_timeSpentInvincible = 0;
			}
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
	public void SetInvincible()
	{
		Invincible = true;
		_timeSpentInvincible = 0;

	}
	public void TakeDamage(int damage)
	{

		Debug.Log("Taking damage: " + damage);
		Health -= damage;
		OnHealthChanged?.Invoke(Health);
		// PlayDamageSound();
		FlashRed();

		if (Health <= 0)
		{
			_willDie = true;
		}
		else
		{
			SetInvincible();
		}
	}

	public void TakeDamageWithForce(int damage, Vector2 force)
	{
		Health -= damage;
		OnHealthChanged?.Invoke(Health);

		// PlayDamageSound();
		FlashRed();

		if (Health <= 0)
		{
			_willDie = true;
		}
		else
		{
			SetInvincible();
			_rb.AddForce(force, ForceMode2D.Impulse);
		}
	}
	void FlashRed()
	{
		if (spriteRenderer != null)
		{
			spriteRenderer.color = Color.red;
			Invoke("ResetColor", 0.1f); // Reset after 0.1 seconds
		}
	}
	void ResetColor()
	{
		if (spriteRenderer != null)
		{
			spriteRenderer.color = originalColor;
		}
	}
}
