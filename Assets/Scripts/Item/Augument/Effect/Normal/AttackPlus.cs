using UnityEngine;

[CreateAssetMenu(menuName = "Augments/Effects/AttackPlus")]
public class AttackPlus : ScriptableObject, IAugmentEffect
{
		public float amount = 1;

		public void Apply(AugmentRuntimeContext ctx)
		{
				if (ctx.Stats != null)
				{
						ctx.Stats.att += amount;
				}
		}

}