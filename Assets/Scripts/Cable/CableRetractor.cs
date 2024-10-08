using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableRetractor : MonoBehaviour
{
	public float speed = 15;
	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		if(Input.GetKey(KeyCode.T))
		{
			Debug.Log("Pulllllll");
			Player.Instance.PauseGravity();
			var step = speed * Time.deltaTime;
			var pullPoint = Cable.Instance.cable.GetPosition(Cable.Instance.cablePositions.Count - 2);
			Player.Instance.transform.position = Vector2.MoveTowards(Player.Instance.transform.position, pullPoint, step);
		}
		else
		{
			Player.Instance.SetGravity();
		}
	}
}
