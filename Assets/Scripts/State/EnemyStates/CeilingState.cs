using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingState : State
{
	public AnimationClip clip;
	public Rigidbody2D Rigidbody;

	public override void Enter() {
		animator.Play(clip.name);
		Rigidbody.gravityScale = 0;
	}

	public override void Do()
	{
		animator.Play(clip.name, 0, time);
		animator.speed = 0;
	}
}