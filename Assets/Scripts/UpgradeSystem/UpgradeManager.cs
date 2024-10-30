using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
		gameObject.SetActive(false);
	}

	// Update is called once per frame
	void Update()
	{
		
	}
	
	public void ShowUI(bool show)
	{
		gameObject.SetActive(show);
	}
}
