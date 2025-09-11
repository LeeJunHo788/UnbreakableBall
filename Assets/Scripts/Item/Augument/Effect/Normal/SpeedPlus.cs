using UnityEngine;

[CreateAssetMenu(menuName = "Augments/Effects/SpeedPlus")]
public class SpeedPlus : ScriptableObject, IAugmentEffect
{
		public float amount = 5;

		public void Apply(AugmentRuntimeContext ctx)
		{
				if(ctx.Stats != null)
				{
						ctx.Stats.moveSpeed += amount;
				}
		}
    
}
