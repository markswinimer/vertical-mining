using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandState : State
{
	public AnimationClip clip;

	public float clipTime = 1f;

	public override void Enter() {
		var clipLength = clip.length;
		var clipSpeed = clipLength / clipTime;
		animator.speed = clipSpeed;
		animator.Play(clip.name);
		StartCoroutine(WaitForEndOfClip());
	}
	public override void Do()
	{
		
	}
	
	private IEnumerator WaitForEndOfClip()
	{
		yield return new WaitForSeconds(clipTime);
		isComplete = true;
	}
}