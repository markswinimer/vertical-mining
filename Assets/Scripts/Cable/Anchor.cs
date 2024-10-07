using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anchor : MonoBehaviour
{
	public bool IsAttached;
	public float AttachRange;
	public int AnchorIndex;
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
}
