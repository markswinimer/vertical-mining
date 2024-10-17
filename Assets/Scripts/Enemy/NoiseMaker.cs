using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMaker : MonoBehaviour
{
	public float NoiseLevel;
	public bool IsMakingNoise;
	
	private void Start() {
		NoiseHandler.Instance.AddNoiseMaker(this);
	}
}
