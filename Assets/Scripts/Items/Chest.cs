using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Chest : MonoBehaviour, IDataPersistence
{
	public Inventory Inventory;

	[SerializeField] private string id;

	[ContextMenu("Generate guid for id")]
	private void GenerateGuid() 
	{
		id = System.Guid.NewGuid().ToString();
	}

	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		
	}
	
	private void OnTriggerEnter2D(Collider2D other) {
		if(other.gameObject.GetComponent<Player>() != null)
		{
			//for testing, just copying inventory
			foreach(var item in Player.Instance.Inventory.Container)
			{
				Inventory.AddItem(item.Item, item.Amount);
			}
		}
	}

	public void LoadData(GameData data)
	{
		var chest = data.Containers.FirstOrDefault(c => c.Id == id);
		Inventory = ScriptableObject.CreateInstance<Inventory>();
		if(chest != null)
		{
			Inventory.Container = chest.Slots;
		}
	}

	public void SaveData(GameData data)
	{
		var chest = data.Containers.FirstOrDefault(c => c.Id == id);
		if(chest != null)
		{
			chest.Slots = Inventory.Container;
		}
		else
		{
			data.Containers.Add(new ChestData(id, Inventory.Container));
		}
	}
	
	private void OnApplicationQuit() {
		Inventory.Container.Clear();
	}
}
