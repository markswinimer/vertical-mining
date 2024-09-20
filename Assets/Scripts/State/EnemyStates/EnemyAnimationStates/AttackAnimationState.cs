using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimationState : State
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
		Debug.Log("Clip should finish");
		isComplete = true;
	}
}