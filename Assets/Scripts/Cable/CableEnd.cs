using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableEnd : MonoBehaviour
{
	public bool IsAttached;
	// Start is called before the first frame update
	void Start()
	{
		if(!IsAttached)
		{
			gameObject.SetActive(false);
		}
	}

	// Update is called once per frame
	void Update()
	{
		
	}
	
	public void AttachCableEnd()
	{
		IsAttached = true;
		gameObject.SetActive(true);
		transform.position = Player.Instance.transform.position;
	}
	
	public void DetachCableEnd()
	{
		IsAttached = false;
		gameObject.SetActive(false);
	}
}
