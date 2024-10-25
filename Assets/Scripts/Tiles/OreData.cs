using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OreData", menuName = "ScriptableObjects/OreData", order = 1)]
public class OreData : ScriptableObject
{
	public float MaxDurability;
	public DrillType DrillType;
	public ItemObject OreToDrop;
}
