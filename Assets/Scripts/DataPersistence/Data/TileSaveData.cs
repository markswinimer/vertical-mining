using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileSaveData
{
	public Vector3Int Position;
	public TileData TileData;
	
	public TileSaveData(Vector3Int position, TileData tileData)
	{
		Position = position;
		TileData = tileData;
	}
}
