using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeHealth : Upgrade
{
	public int HealthUpgradeAmount;
	public override void PerformUpgrade()
	{
		Player.Instance.MaxHealth += HealthUpgradeAmount;
	}
}
