using System;
using System.Collections.Generic;
using UnityEngine;

public class Cable : MonoBehaviour, IDataPersistence
{
	public static Cable Instance { get; private set; }
	public event Action<bool> OnCableAttached;

	public Transform player;

	public LineRenderer cable;
	public LayerMask collMask;

	public List<Vector3> cablePositions { get; set; } = new List<Vector3>();
	public float ignoreDistance;
	public int LastAnchorIndex;
	public CableEnd CableEnd;
	public bool IsAttachedToPlayer = true;
	public float MaxLength = 50;
	public float CurrentLength;

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	private void Start()
	{
		OnCableAttached?.Invoke(IsAttachedToPlayer);
	}

	private void Update()
	{
		UpdateCablePositions();
		if(IsAttachedToPlayer)
		{
			LastSegmentGoToPlayerPos();
			DetectCollisionEnter();
			if (cablePositions.Count > 2) DetectCollisionExits();   
		}
		else
		{
			LastSegmentGoToCableEndPos();
		}
		UpdateCableLength();
		UpdateDistanceJoint();
	
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
		if(cablePositions.Count > 0)
		{
			cablePositions.RemoveAt(cablePositions.Count - 1);
		}
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
	
	public void DropCable()
	{
		IsAttachedToPlayer = false;
		OnCableAttached?.Invoke(false);
		CableEnd.AttachCableEnd();
	}
	
	public void PickupCable()
	{
		IsAttachedToPlayer = true;
		OnCableAttached?.Invoke(true);
		CableEnd.DetachCableEnd();
	}
	
	public void UpdateCableLength()
	{
		var length = 0f;
		
		if (cable.positionCount < 2) return;

		// Iterate through each pair of consecutive points
		for (int i = 0; i < cable.positionCount - 1; i++)
		{
			// Get the positions of two consecutive points
			Vector3 pointA = cable.GetPosition(i);
			Vector3 pointB = cable.GetPosition(i + 1);

			// Calculate the distance between these two points and add to the total length
			length += Vector3.Distance(pointA, pointB);
		}

		CurrentLength = length;
	}
	
	public void UpdateDistanceJoint()
	{
		Player.Instance.DistanceJoint.connectedAnchor = cable.GetPosition(cable.positionCount - 2);
		var distance = Mathf.Round(MaxLength - CurrentLength + Vector3.Distance(cable.GetPosition(cablePositions.Count - 1), cable.GetPosition(cablePositions.Count - 2)));
		Debug.Log("Disance = "+ distance);
		Player.Instance.DistanceJoint.distance = distance;
	}

	private void LastSegmentGoToPlayerPos() => cable.SetPosition(cable.positionCount - 1, player.position);
	private void LastSegmentGoToCableEndPos() => cable.SetPosition(cable.positionCount - 1, CableEnd.transform.position);

	public void LoadData(GameData data)
	{
		var cableData = data.CableData;
		if(cableData?.CablePositions?.Count > 0)
		{
			cablePositions = cableData.CablePositions;
			LastAnchorIndex = cableData.LastAnchorIndex;
			IsAttachedToPlayer = cableData.IsAttachedToPlayer;
			CableEnd.transform.position = cableData.CableEndPosition;
			if(!IsAttachedToPlayer)
			{
				Debug.Log("IsAttached False, setup cable end");
				CableEnd.IsAttached = true;
				CableEnd.gameObject.SetActive(true);
			}
			UpdateCablePositions();
		}
		else
		{
			Debug.Log("Add Pos Load");
			AddPosToCable(transform.position);
			IsAttachedToPlayer = true;
		}
	}

	public void SaveData(GameData data)
	{
		data.CableData.CablePositions = cablePositions;
		data.CableData.LastAnchorIndex = LastAnchorIndex;
		data.CableData.IsAttachedToPlayer = IsAttachedToPlayer;
		data.CableData.CableEndPosition = CableEnd.transform.position;
	}
}
