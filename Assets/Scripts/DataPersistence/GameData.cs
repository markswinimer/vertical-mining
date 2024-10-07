using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
	public string LastUpdated;
	public PlayerData PlayerData;
	public CableData CableData;
	public List<ChestData> Containers;
	public List<AnchorData> Anchors;
	public List<TileSaveData> TileData;
	
	public GameData()
	{
		PlayerData = new PlayerData();
		Containers = new List<ChestData>();
		TileData = new List<TileSaveData>();
		Anchors = new List<AnchorData>();
		CableData = new CableData();
	}
}
