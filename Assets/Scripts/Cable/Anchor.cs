using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Anchor : MonoBehaviour, IDataPersistence
{
	public bool IsAttached;
	public float AttachRange;
	public int AnchorIndex;
	public string id;

	[ContextMenu("Generate guid for id")]
	private void GenerateGuid() 
	{
		id = System.Guid.NewGuid().ToString();
	}
	// Start is called before the first frame update
	void Awake()
	{
		if(Vector2.Distance(Cable.Instance.transform.position, transform.position) < AttachRange)
		{
			AttachCable();
		}
	}

	// Update is called once per frame
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.R))
		{
			Debug.Log("Key Down R");
			if(Vector2.Distance(Player.Instance.transform.position, transform.position) < AttachRange)
			{
				Debug.Log("Is Close Enough");
				if(!IsAttached)
				{
					AttachCable();
				}
				else
				{
					DetachCable();
				}
			}
		}
	}
	
	public void AttachCable()
	{
		AnchorIndex = Cable.Instance.AddAnchorToCable(transform.position);
		IsAttached = true;
	}
	
	public void DetachCable()
	{
		Cable.Instance.RemoveAnchorFromCable(AnchorIndex);
		IsAttached = false;
	}

	public void LoadData(GameData data)
	{
		var anchor = data.Anchors.FirstOrDefault(c => c.Id == id);
		if(anchor != null)
		{
			IsAttached = anchor.IsAttached;
			AnchorIndex = anchor.AnchorIndex;
		}
	}

	public void SaveData(GameData data)
	{
		var anchor = data.Anchors.FirstOrDefault(c => c.Id == id);
		if(anchor != null)
		{
			anchor.IsAttached = IsAttached;
			anchor.AnchorIndex = AnchorIndex;
		}
		else
		{
			data.Anchors.Add(new AnchorData(id, IsAttached, AnchorIndex));
		}
	}
}
