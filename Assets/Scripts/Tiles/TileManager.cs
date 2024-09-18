using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class TileManager : MonoBehaviour
{
    public static TileManager Instance { get; private set; }

    private Dictionary<Vector3Int, TileData> _tileDataDictionary;
    private Tilemap _tilemap;

    public TilemapBlockBreaker tileBreakHandler;
    public RuntimeAnimatorController tileBreakAnimationController;
    public AnimationClip clip;

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


    public void DamageTile(Vector3Int position, float damage)
    {
        if (_tileDataDictionary.ContainsKey(position))
        {
            TileData tileData = _tileDataDictionary[position];
            
            if (!tileData.isBreaking)
            {
                InstantiateAnimation(position, tileData);
            } 
            tileData.DamageTile(damage);
        }
    }

    void InstantiateAnimation(Vector3Int position, TileData tileData)
    {
        Vector3 worldPosition = _tilemap.CellToWorld(position);

        // offset the x value to account for the tilemap anchor (bottom left? .5, .5 is the value)
        Vector3 centeredPosition = worldPosition + new Vector3(_tilemap.tileAnchor.x, 0, 0);

        TilemapBlockBreaker tileBreakHandler = Instantiate(this.tileBreakHandler, centeredPosition, Quaternion.identity);
        
        // appear in front
        tileBreakHandler.transform.position = new Vector3(centeredPosition.x, centeredPosition.y, -0.1f);

        tileData.SetBreakHandler(tileBreakHandler, position);
    }

    public void DestroyTile(Vector3Int position)
    {
        _tilemap.SetTile(position, null);
        _tileDataDictionary.Remove(position);
    }

}

public class TileData
{
    public bool isBreaking;
    public float maxDurability = 100f;
    public float durability;
    private TilemapBlockBreaker _breakHandler;
    private Vector3Int _position;

    public void SetBreakHandler(TilemapBlockBreaker breakHandler, Vector3Int position)
    {
        durability = maxDurability;
        _position = position;
        _breakHandler = breakHandler;
        breakHandler.Instantiate(this);

        isBreaking = true;
    }

    public void DamageTile(float damage)
    {
        durability -= damage;
        if (durability <= 0)
        {
            // get tile on the tilemap from position and destroy it and destroy break animation gameobject
            TileManager.Instance.DestroyTile(_position);
            _breakHandler.DestroyHandler();
        }

        float durabilityPercentage = durability / maxDurability;
        _breakHandler.UpdateAnimation(durabilityPercentage);
    }

    public void RegenerateDurability()
    {
        float regenerationAmount = maxDurability / 3f;

        durability += regenerationAmount;
        durability = Mathf.Clamp(durability, 0, maxDurability);

        if (durability >= maxDurability)
        {
            // Get tile on the tilemap from position and destroy it
            _breakHandler.DestroyHandler();
            _breakHandler = null;
            isBreaking = false;
        }
        else
        {
            float durabilityPercentage = durability / maxDurability;
            _breakHandler.UpdateAnimation(durabilityPercentage);
        }
    }
}
