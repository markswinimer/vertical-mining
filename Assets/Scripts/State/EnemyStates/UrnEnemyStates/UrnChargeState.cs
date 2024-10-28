using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UrnChargingState : State
{
	public AnimationClip clip;

	public override void Enter()
	{
		animator.Play(clip.name);
	}
	public override void Do()
	{
		//rotate transform slowly
		transform.Rotate(0, 0, 1);
	}
}