using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NoiseHandler : MonoBehaviour
{
	public List<NoiseMaker> NoiseMakers;
	public static NoiseHandler Instance { get; private set; }
	// Start is called before the first frame update
	void Start()
	{
		NoiseMakers = new List<NoiseMaker>();
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}
	
	public List<NoiseMaker> GetActiveNoises()
	{
		return NoiseMakers.Where(n => n.IsMakingNoise).OrderBy(n => n.NoiseLevel).ToList();
	}
	
	public void AddNoiseMaker(NoiseMaker noiseMaker)
	{
		if(NoiseMakers.Contains(noiseMaker)) return;
		NoiseMakers.Add(noiseMaker);
	}

	void OnDrawGizmos()
	{
#if UNITY_EDITOR
		if (Application.isPlaying)
		{
			foreach(var noise in GetActiveNoises())
			{
				UnityEditor.Handles.DrawWireDisc(noise.transform.position, Vector3.back, noise.NoiseLevel);
			}
		}
#endif
	}
}
