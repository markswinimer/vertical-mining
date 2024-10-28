using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BarrierManager : MonoBehaviour
{
	public int Length;
	public int Height;
	public OreRuleTile BarrierTileType;
	
	public Dictionary<Vector3Int, OreRuleTile> BarrierTiles;
	
	public List<Vector3Int> DestroyedTilePos;
	public List<int> DestroyedColumn;
	public int XGapSizeOffset;
	// Start is called before the first frame update
	void Start()
	{
		BarrierTiles = new Dictionary<Vector3Int, OreRuleTile>();
		DestroyedTilePos = new List<Vector3Int>();
		DestroyedColumn = new List<int>();
		TileManager.Instance.Barriers.Add(this);
	}
	
	public void InitializeData() 
	{
		var bounds = new BoundsInt((int)transform.position.x - (Length/2),
		(int) transform.position.y - (Height/2), -1, (int) transform.position.x + (Length/2), (int) transform.position.y + (Height/2), 1);
		var tilesInArea = TileManager.Instance.Tilemap.GetTilesBlock(bounds);
		var allPos = TileManager.Instance.Tilemap.cellBounds.allPositionsWithin;
		
		foreach(var pos in allPos)
		{
			var tile = TileManager.Instance.Tilemap.GetTile(pos);
			Debug.Log("allpos check + null " + (tile ==null).ToString());
			if(tile != null && tile is OreRuleTile oreTile)
			{
				Debug.Log("tile check");
				if(oreTile == BarrierTileType && tilesInArea.Contains(tile)) BarrierTiles.Add(pos, oreTile);
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
		
		var yPos = pos.y;
		var tilesInColumn = BarrierTiles.Where(k => k.Key.y == yPos);
		if(tilesInColumn.All(t => DestroyedTilePos.Contains(t.Key)))
		{
			DestroyedColumn.Add(yPos);
		}
		
		if(DestroyedColumn.Count > 2)
		{
			DestroyedColumn.Sort();
			var middle = DestroyedColumn[DestroyedColumn.Count / 2];
			
			var lowerBound = middle - XGapSizeOffset;
			var upperBound = middle + XGapSizeOffset;
			var tilesToDestroy = new List<Vector3Int>();
			foreach(var tile in BarrierTiles.Where(k => lowerBound <= k.Key.y && k.Key.y <= upperBound))
			{
				tilesToDestroy.Add(tile.Key);
			}
			
			TileManager.Instance.DestroyTiles(tilesToDestroy);
		}
	}

	void OnDrawGizmos()
	{
#if UNITY_EDITOR
		Gizmos.DrawLine(new Vector3(transform.position.x - (Length/2), transform.position.y - (Height/2)), new Vector3(transform.position.x + (Length/2), transform.position.y - (Height/2), 0));
		Gizmos.DrawLine(new Vector3(transform.position.x - (Length/2), transform.position.y + (Height/2)), new Vector3(transform.position.x + (Length/2), transform.position.y + (Height/2), 0));
#endif
	}
}