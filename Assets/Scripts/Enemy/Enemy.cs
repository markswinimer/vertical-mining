using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
	public Transform Target;
	private NavMeshAgent _agent;
	// Start is called before the first frame update
	void Start()
	{
		_agent = GetComponent<NavMeshAgent>();
		_agent.updateRotation = false;
		_agent.updateUpAxis = false;
		
		Target = FindFirstObjectByType<Player>().transform;
	}

	// Update is called once per frame
	void Update()
	{
		_agent.SetDestination(Target.position);
	}
}
