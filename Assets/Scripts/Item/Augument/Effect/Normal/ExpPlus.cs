using UnityEngine;

[CreateAssetMenu(menuName = "Augments/Effects/ExpPlus")]
public class ExpPlus : ScriptableObject, IAugmentEffect
{
		public float amount = 0.1f;

		public void Apply(AugmentRuntimeContext ctx)
		{
				if (ctx.Stats != null)
				{
						ctx.Stats.expGain += amount;
				}
		}

}