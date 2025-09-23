using UnityEngine;

[CreateAssetMenu(menuName = "Augments/Effects/Upgrade/Spread_UpgradeInterval")]
public class Spread_UpgradeInterval : ScriptableObject, IAugmentEffect
{
		public void Apply(AugmentRuntimeContext ctx)
		{
				ctx.manager.spreadAM.interval -= 0.25f;

				if (ctx.manager.spreadAM.interval < 0.05f)
						ctx.manager.spreadAM.interval = 0.05f;


		}
}
