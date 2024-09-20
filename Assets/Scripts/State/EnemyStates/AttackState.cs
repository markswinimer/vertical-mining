using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
	public AttackAnimationState animation;
	public IdleState idleAnimation;
	public float hitRange = 1;
	public int damage;
	private bool _onCooldown;
	public float cooldown;
	public float chargeTime;
	private Coroutine _attackRoutine;

	public override void Enter()
	{
		animation.clipTime = chargeTime;
	}

	public override void Do()
	{
		if(!_onCooldown)
		{
			_attackRoutine = StartCoroutine(Attack());
		}
		if(!IsWithinReach())
		{
			Debug.Log("Attack complete");
			isComplete = true;
		} 
	}

	public override void FixedDo()
	{
		
	}

	public override void Exit()
	{
		Debug.Log("Exit attack");
		StopCoroutine(_attackRoutine);
		_onCooldown = false;
	}

	public IEnumerator Attack()
	{
		Debug.Log("Start Attack");
		_onCooldown = true;
		Set(animation, true);
		Debug.Log("Charge Wait");
		yield return new WaitForSeconds(chargeTime);
		Debug.Log("Deal Damage check");
		if(Vector2.Distance(core.transform.position, Player.Instance.transform.position) < hitRange)
		{
			Debug.Log("Deal damage");
			Player.Instance.DealDamage(damage);
		}
		Debug.Log("cooldown Wait");
		Set(idleAnimation, true);
		yield return new WaitForSeconds(cooldown);
		Debug.Log("cooldown done");
		_onCooldown = false;
		
	}

	public bool IsWithinReach()
	{
		return Vector2.Distance(core.transform.position, Player.Instance.transform.position) < hitRange;
	}
}