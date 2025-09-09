using UnityEngine;

[CreateAssetMenu(menuName = "Augments/Effects/FlatAttackBonus")]
public class AttackBonusEffect : ScriptableObject, IAugmentEffect
		{
		public int amount = 10;

		public void Apply(AugmentRuntimeContext ctx)
				{
				if (ctx.Stats != null)
						{
						ctx.Stats.att += amount;
						}
				}
		}