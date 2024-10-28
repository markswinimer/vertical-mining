using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BarrierManager : MonoBehaviour
{
	public OreRuleTile BarrierTileType;
	
	public Dictionary<Vector3Int, OreRuleTile> BarrierTiles;
	public Tilemap Tilemap;
	public List<Vector3Int> DestroyedTilePos;
	public List<int> DestroyedColumn;
	public int XGapSizeOffset;
	// Start is called before the first frame update
	void Start()
	{
	}
	
	public void InitializeData() 
	{
		BarrierTiles = new Dictionary<Vector3Int, OreRuleTile>();
		DestroyedTilePos = new List<Vector3Int>();
		DestroyedColumn = new List<int>();
		var allPos = Tilemap.cellBounds.allPositionsWithin;
		Debug.Log("Doing allpos");
		foreach(var pos in allPos)
		{
			var tile = Tilemap.GetTile(pos);
			Debug.Log("allpos check + null " + (tile ==null).ToString());
			if(tile != null && tile is OreRuleTile oreTile)
			{
				Debug.Log("tile check - type == " + (oreTile == BarrierTileType).ToString());
				if(oreTile == BarrierTileType)
				{
					BarrierTiles.Add(pos, oreTile);
					Debug.Log("Add tile to barrier");
				}
			}
		}
	}

	// Update is called once per frame
	void Update()
	{
		
	}
	
	public void DestroyBarrierTile(Vector3Int pos)
	{
		Debug.Log("Destory barrier check + count = " + BarrierTiles.Count);
		if(!BarrierTiles.ContainsKey(pos)) return;
		Debug.Log("Contains barrier check");
		DestroyedTilePos.Add(pos);
		
		var xPos = pos.x;
		var tilesInColumn = BarrierTiles.Where(k => k.Key.x == xPos);
		Debug.Log("tiles in column at x pos " + xPos + " = " + tilesInColumn.Count());
		if(tilesInColumn.All(t => DestroyedTilePos.Contains(t.Key)))
		{
			DestroyedColumn.Add(xPos);
		}
		
		if(DestroyedColumn.Count > 2)
		{
			Debug.Log("Break em all");
			DestroyedColumn.Sort();
			var middle = DestroyedColumn[DestroyedColumn.Count / 2];
			
			var lowerBound = middle - XGapSizeOffset;
			var upperBound = middle + XGapSizeOffset;
			var tilesToDestroy = new List<Vector3Int>();
			foreach(var tile in BarrierTiles.Where(k => lowerBound <= k.Key.x && k.Key.x <= upperBound))
			{
				Debug.Log("adding tile to destroy");
				tilesToDestroy.Add(tile.Key);
			}
			
			TileManager.Instance.DestroyTiles(tilesToDestroy);
		}
	}
}