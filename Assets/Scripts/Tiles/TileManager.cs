using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

public class TileManager : MonoBehaviour, IDataPersistence
{
	public static TileManager Instance { get; private set; }

	private Dictionary<Vector3Int, TileData> _tileDataDictionary;
	private Dictionary<Vector3Int, TileInstance> _tileInstanceDictionary;
	public Tilemap TileMap;

	public TileInstance tileInstance;
	public List<BarrierManager> Barriers;

	// this sets up a global reference to the tilmap
	// eventually this file will manage multiple tilemaps
	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
			return;
		}
		Barriers = FindObjectsByType<BarrierManager>(FindObjectsSortMode.None).ToList();

		Instance = this;
	}

	void Start()
	{
	}

	void InitializeTileData()
	{
		foreach (Vector3Int position in TileMap.cellBounds.allPositionsWithin)
		{
			if (TileMap.HasTile(position))
			{
				var tileBase = TileMap.GetTile(position);
				TileData tileData = GetTileDataFromOre(tileBase);
				_tileDataDictionary[position] = tileData;
			}
		}
		Barriers.ForEach(b => b.InitializeData());
	}

	public Vector3Int GetTilemapWorldToCell(Vector3 worldPosition)
	{
		return TileMap.WorldToCell(worldPosition);
	}

	public bool IsTileValid(Vector3Int position)
	{
		return _tileDataDictionary.ContainsKey(position);
	}

	public bool IsTileInstanceValid(Vector3Int position)
	{
		return _tileInstanceDictionary.ContainsKey(position);
	}

	void InstantiateTile(Vector3Int position)
	{
		Vector3 worldPosition = TileMap.CellToWorld(position);

		// offset the x value to account for the tilemap anchor (bottom left? .5, .5 is the value)
		Vector3 centeredPosition = worldPosition + new Vector3(TileMap.tileAnchor.x, 0, 0);

		TileInstance tileInstance = Instantiate(this.tileInstance, centeredPosition, Quaternion.identity);
		tileInstance.transform.position = new Vector3(centeredPosition.x, centeredPosition.y, -0.1f);
		var tileBase = TileMap.GetTile(position);
		if(tileBase is OreRuleTile oreTile)
		{
			tileInstance.dropPrefab = oreTile.OreData.OreToDrop;
		}
		else
		{
			tileInstance.dropPrefab = null;
		}
		

		tileInstance.Instantiate(position);

		_tileInstanceDictionary[position] = tileInstance;
	}

	public void OnDamageTile(Vector3Int position, float damage, DrillType drillSource)
	{
		TileData tileData = TryGetTileData(position);

		if (tileData != null)
		{
			if (!IsTileInstanceValid(position))
			{
				InstantiateTile(position);
			}

			TileInstance tileInstance = TryGetTileInstance(position);

			if (tileInstance != null)
			{
				// update tiles durability, and return new durablity
				float durability = tileData.UpdateDurability(-damage, drillSource);

				if (durability <= 0)
				{
					DestroyTile(position);
				}
				else
				{
					float damagePercentage = durability / tileData.MaxDurability;
					tileInstance.ProcessDamageTile(damagePercentage);
				}
			}
		}
	}
	
	public void DestroyTiles(List<Vector3Int> positions) => positions.ForEach(DestroyTile);
	
	public void DestroyTile(Vector3Int position)
	{
		TileInstance tileInstance = TryGetTileInstance(position);
		_tileDataDictionary.Remove(position);
		_tileInstanceDictionary.Remove(position);

		TileMap.SetTile(position, null);
		if(tileInstance != null)
		{
			tileInstance.ProcessDestroyTile();
			Debug.Log("DestroyTile");
			Barriers.ForEach(b => b.DestroyBarrierTile(position));
		}
	}

	// this function being called by the child isn't ideal, but it works for now
	public void RestoreTileDurability(TileInstance tileInstance, Vector3Int position)
	{
		TileData tileData = TryGetTileData(position);

		if (tileData != null)
		{
			float maxDurability = tileData.MaxDurability;
			float durability = tileData.UpdateDurability(maxDurability / 3, DrillType.Restore);

			if (durability == maxDurability)
			{
				_tileInstanceDictionary.Remove(position);
				Destroy(tileInstance.gameObject);
			}
			else
			{
				float damagePercentage = durability / maxDurability;
				tileInstance.ProcessRestoreDurability(damagePercentage);
			}
		}
	}

	public TileData TryGetTileData(Vector3Int position)
	{
		if (IsTileValid(position))
		{
			return _tileDataDictionary[position];
		}
		return null;
	}

	public TileInstance TryGetTileInstance(Vector3Int position)
	{
		if (IsTileInstanceValid(position))
		{
			return _tileInstanceDictionary[position];
		}
		return null;
	}

	public void LoadData(GameData data)
	{
		_tileDataDictionary = new Dictionary<Vector3Int, TileData>();
		_tileInstanceDictionary = new Dictionary<Vector3Int, TileInstance>();
		if(data.TileData?.Count == 0)
		{
			InitializeTileData();
		}
		else
		{
			_tileDataDictionary.Clear();
			foreach(var tile in data.TileData)
			{
				_tileDataDictionary.Add(tile.Position, tile.TileData);
			}
			DestroyDeadTiles();
		}
	}
	
	private void DestroyDeadTiles()
	{
		foreach (Vector3Int position in TileMap.cellBounds.allPositionsWithin)
		{
			if (TileMap.HasTile(position) && !_tileDataDictionary.ContainsKey(position))
			{
				TileMap.SetTile(position, null);
			}
		}
	}

	public void SaveData(GameData data)
	{
		data.TileData.Clear();
		data.TileData.AddRange(_tileDataDictionary.Select(t => new TileSaveData(t.Key, t.Value)));
	}
	
	public TileData GetTileDataFromOre(TileBase tileBase)
	{
		if(tileBase is OreRuleTile oreTile)
		{
			var oreData = oreTile.OreData;
			return new TileData(oreData.MaxDurability, oreData.DrillType);
		}
		else
		{
			return new TileData(100, DrillType.Any);
		}
	}
}

[System.Serializable]
public class TileData
{
	public float MaxDurability = 100f;
	public float Durability;
	public DrillType DrillType;
	
	public TileData(float maxDurability, DrillType drillType)
	{
		MaxDurability = maxDurability;
		DrillType = drillType;
		Durability = MaxDurability;
	}

	public float UpdateDurability(float value, DrillType drillSource)
	{
		if(DrillType != DrillType.Any && drillSource != DrillType && drillSource != DrillType.Restore) return Durability;
		// adds or subtracts value from durability, and clamps it between 0 and maxDurability
		Durability = Mathf.Clamp(Durability + value, 0, MaxDurability);

		return Durability;
	}
}