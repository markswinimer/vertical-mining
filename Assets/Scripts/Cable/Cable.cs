using System.Collections.Generic;
using UnityEngine;

public class Cable : MonoBehaviour, IDataPersistence
{
	public static Cable Instance { get; private set; }
	
	public Transform player;

	public LineRenderer cable;
	public LayerMask collMask;

	public List<Vector3> cablePositions { get; set; } = new List<Vector3>();
	public float ignoreDistance;
	public int LastAnchorIndex;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
	} 

	private void Update()
	{
		UpdateCablePositions();
		LastSegmentGoToPlayerPos();

		DetectCollisionEnter();
		if (cablePositions.Count > 2) DetectCollisionExits();       
		Debug.Log("Cable pos count = "+ cablePositions.Count); 
	}

	private void DetectCollisionEnter()
	{
		var prevCablePos = cable.GetPosition(cablePositions.Count - 2);
		var hit = Physics2D.Linecast(player.position, prevCablePos, collMask);
		if (hit.collider != null && Vector2.Distance(hit.point, prevCablePos) > ignoreDistance)
		{
			var cablePosToAdd = hit.point + hit.normal * 0.2f;
			cablePositions.RemoveAt(cablePositions.Count - 1);
			AddPosToCable(cablePosToAdd);
		}
	}

	private void DetectCollisionExits()
	{
		if(LastAnchorIndex == cablePositions.Count - 2) return;
		var hit = Physics2D.Linecast(player.position, cable.GetPosition(cablePositions.Count - 3), collMask);
		if (hit.collider == null)
		{
			cablePositions.RemoveAt(cablePositions.Count - 2);
		}
	}

	private void AddPosToCable(Vector3 _pos)
	{
		cablePositions.Add(_pos);
		cablePositions.Add(player.position); //Always the last pos must be the player
	}

	private void UpdateCablePositions()
	{
		cable.positionCount = cablePositions.Count;
		cable.SetPositions(cablePositions.ToArray());
	}
	
	public int AddAnchorToCable(Vector3 position)
	{
		cablePositions.RemoveAt(cablePositions.Count - 1);
		cablePositions.Add(position);
		cablePositions.Add(player.position); //Always the last pos must be the player
		LastAnchorIndex = cablePositions.Count - 2;
		return LastAnchorIndex;
	}
	
	public void RemoveAnchorFromCable(int index)
	{
		if(index != LastAnchorIndex || index == 0) return;
		var cablePos = cablePositions.Count;
		for(int i = cablePos - 1; i >= index; i--)
		{
			cablePositions.RemoveAt(i);
		}
		cablePositions.Add(player.position);
	}

	private void LastSegmentGoToPlayerPos() => cable.SetPosition(cable.positionCount - 1, player.position);

	public void LoadData(GameData data)
	{
		var cableData = data.CableData;
		if(cableData?.CablePositions?.Count == 0)
		{
			Debug.Log("Add Pos Load");
			AddPosToCable(transform.position);
		}
		else
		{
			cablePositions = cableData.CablePositions;
			LastAnchorIndex = cableData.LastAnchorIndex;
			UpdateCablePositions();
		}
	}

	public void SaveData(GameData data)
	{
		data.CableData.CablePositions = cablePositions;
		data.CableData.LastAnchorIndex = LastAnchorIndex;
	}
}
