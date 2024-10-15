using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NoiseDetection : EnemyDetection
{
	public float EnemyDetectionRange;
	public bool ShouldKeepChasing;
	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		
	}
	
	public override bool ShouldChaseTarget()
	{
		var noises = NoiseHandler.Instance.GetActiveNoises();
		foreach(var noise in noises)
		{
			if(Vector2.Distance(transform.position, noise.transform.position) < EnemyDetectionRange + noise.NoiseLevel)
			{
				Target = noise;
				return true;
			}
		}
		return false;
	}
	
	public override bool ShouldStayInChase()
	{
		if(Target.IsMakingNoise &&(Vector2.Distance(transform.position, Target.transform.position) < EnemyDetectionRange + Target.NoiseLevel))
		{
			return true;
		}
		return false;
	}

	void OnDrawGizmos()
	{
#if UNITY_EDITOR
		if (Application.isPlaying)
		{
			UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.back, EnemyDetectionRange);
		}
#endif
	}
}
