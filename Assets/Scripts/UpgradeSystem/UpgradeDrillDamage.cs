using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeDrillSpeed : Upgrade
{
	public float DrillDamageUpgradeAmount;
	public override void PerformUpgrade()
	{
		DrillMachine.Instance.DrillDamage += DrillDamageUpgradeAmount;
	}
}
