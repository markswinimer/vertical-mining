using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
	public NoiseMaker Target;
	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		
	}
	
	public virtual bool ShouldChaseTarget()
	{
		Debug.Log("Implement Chase Logic");
		return false;
	}
	
	public virtual bool ShouldStayInChase()
	{
		Debug.Log("Implement Chase Logic");
		return false;
	}
}
