using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
	public string LastUpdated;
	public PlayerData PlayerData;
	public List<ChestData> Containers;
	public List<TileSaveData> TileData;
	
	public GameData()
	{
		PlayerData = new PlayerData();
		Containers = new List<ChestData>();
		TileData = new List<TileSaveData>();
	}
}
