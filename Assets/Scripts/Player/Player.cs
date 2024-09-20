using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	// singleton instance
	public static Player Instance { get; private set; }
   
	public float AttackSpeed  = .5f;
	
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
}
