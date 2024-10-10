using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableRetractor : MonoBehaviour
{
	public float speed = 15;
	public float AttachRange = 5;
	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		if(Input.GetKey(KeyCode.T) && Cable.Instance.IsAttachedToPlayer)
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
		
		if(Input.GetKeyDown(KeyCode.Q))
		{
			if(Cable.Instance.IsAttachedToPlayer)
			{
				Cable.Instance.DropCable();
			}
			else if(Vector2.Distance(Player.Instance.transform.position, Cable.Instance.CableEnd.transform.position) < AttachRange)
			{
				Cable.Instance.PickupCable();
			}
		}
	}
}
