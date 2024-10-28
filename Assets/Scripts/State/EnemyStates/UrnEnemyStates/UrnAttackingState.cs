using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UrnAttackingState : State
{
	public AttackAnimationState animation;
	public IdleState idleAnimation;
	public float hitRange = 1;
	public int damage;
	public bool _onCooldown;
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
			isComplete = true;
		} 
	}

	public override void FixedDo()
	{
		
	}

	public override void Exit()
	{
		StopCoroutine(_attackRoutine);
		_onCooldown = false;
	}

	public IEnumerator Attack()
	{
		var enemyDirectionLocal = Player.Instance.transform.TransformPoint(transform.position);
		core.transform.localScale = new Vector3(Mathf.Sign(enemyDirectionLocal.x), 1, 1);
		_onCooldown = true;
		Set(animation, true);
		yield return new WaitForSeconds(chargeTime);
		if(Vector2.Distance(core.transform.position, Player.Instance.transform.position) < hitRange)
		{
			Player.Instance.DealDamage(damage);
		}
		Set(idleAnimation, true);
		yield return new WaitForSeconds(cooldown);
		_onCooldown = false;
		
	}

	public bool IsWithinReach()
	{
		return Vector2.Distance(core.transform.position, Player.Instance.transform.position) < hitRange;
	}
}