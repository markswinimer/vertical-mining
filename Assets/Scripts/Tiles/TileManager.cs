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
	public Tilemap _tilemap;

	public TileInstance tileInstance;

	// this sets up a global reference to the tilmap
	// eventually this file will manage multiple tilemaps
	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
			return;
		}

		Instance = this;
	}

	void Start()
	{
		_tileDataDictionary = new Dictionary<Vector3Int, TileData>();
		_tileInstanceDictionary = new Dictionary<Vector3Int, TileInstance>();
	}

	void InitializeTileData()
	{
		foreach (Vector3Int position in _tilemap.cellBounds.allPositionsWithin)
		{
			if (_tilemap.HasTile(position))
			{
				TileData tileData = new TileData();
				tileData.durability = 100f;
				_tileDataDictionary[position] = tileData;
			}
		}
	}

	public Vector3Int GetTilemapWorldToCell(Vector3 worldPosition)
	{
		return _tilemap.WorldToCell(worldPosition);
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
		Vector3 worldPosition = _tilemap.CellToWorld(position);

		// offset the x value to account for the tilemap anchor (bottom left? .5, .5 is the value)
		Vector3 centeredPosition = worldPosition + new Vector3(_tilemap.tileAnchor.x, 0, 0);

		TileInstance tileInstance = Instantiate(this.tileInstance, centeredPosition, Quaternion.identity);
		tileInstance.transform.position = new Vector3(centeredPosition.x, centeredPosition.y, -0.1f);

		tileInstance.Instantiate(position);

		_tileInstanceDictionary[position] = tileInstance;
	}

	public void OnDamageTile(Vector3Int position, float damage)
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
				float durability = tileData.UpdateDurability(-damage);

				if (durability <= 0)
				{
					_tileDataDictionary.Remove(position);
					_tileInstanceDictionary.Remove(position);

					_tilemap.SetTile(position, null);
					tileInstance.ProcessDestroyTile();
				}
				else
				{
					float damagePercentage = durability / tileData.maxDurability;
					tileInstance.ProcessDamageTile(damagePercentage);
				}
			}
		}
	}

	// this function being called by the child isn't ideal, but it works for now
	public void RestoreTileDurability(TileInstance tileInstance, Vector3Int position)
	{
		TileData tileData = TryGetTileData(position);

		if (tileData != null)
		{
			float maxDurability = tileData.maxDurability;
			float durability = tileData.UpdateDurability(maxDurability / 3);

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
		}
	}

	public void SaveData(GameData data)
	{
		data.TileData.Clear();
		data.TileData.AddRange(_tileDataDictionary.Select(t => new TileSaveData(t.Key, t.Value)));
	}
}

public class TileData
{
	public float maxDurability = 100f;
	public float durability;

	public float UpdateDurability(float value)
	{
		// adds or subtracts value from durability, and clamps it between 0 and maxDurability
		durability = Mathf.Clamp(durability + value, 0, maxDurability);
		Debug.Log("Updating durability: " + durability + " with value: " + value);

		return durability;
	}
}