using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class TileManager : MonoBehaviour
{
    public static TileManager Instance { get; private set; }

    private Dictionary<Vector3Int, TileData> _tileDataDictionary;
    private Tilemap _tilemap;

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
        _tilemap = Object.FindFirstObjectByType<Tilemap>();
        _tileDataDictionary = new Dictionary<Vector3Int, TileData>();

        InitializeTileData();
    }

    void InitializeTileData()
    {
        foreach (Vector3Int position in _tilemap.cellBounds.allPositionsWithin)
        {
            if (_tilemap.HasTile(position))
            {
                TileData tileData = new TileData();
                tileData.durability = 100f; // Default durability, customize as needed
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

    public void DamageTile(Vector3Int position, float damage)
    {
        if (_tileDataDictionary.ContainsKey(position))
        {
            _tileDataDictionary[position].durability -= damage;

            if (_tileDataDictionary[position].durability <= 0)
            {
                DestroyTile(position);
            }
        }
    }

    void DestroyTile(Vector3Int position)
    {
        _tilemap.SetTile(position, null); // Remove the tile from the tilemap
        _tileDataDictionary.Remove(position); // Remove the data for this tile
    }
}

public class TileData
{
    public float durability;
    // Add other properties as needed (e.g., type, hardness, etc.)
}