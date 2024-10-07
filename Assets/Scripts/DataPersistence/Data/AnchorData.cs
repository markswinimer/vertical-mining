using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnchorData
{
	public string Id;
	public bool IsAttached;
	public int AnchorIndex;
	
	public AnchorData(string id, bool isAttached, int anchorIndex)
	{
		Id = id;
		IsAttached = isAttached;
		AnchorIndex = anchorIndex;
	}
}
