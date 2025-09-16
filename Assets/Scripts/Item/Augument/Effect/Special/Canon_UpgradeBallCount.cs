using UnityEngine;

[CreateAssetMenu(menuName = "Augments/Effects/Upgrade/Canon_UpgradeBallCount")]
public class Canon_UpgradeBallCount : ScriptableObject, IAugmentEffect
{
		public int amount = 1;

		public void Apply(AugmentRuntimeContext ctx)
		{
				ctx.manager.canonAM.canonBallCount += amount;
		}
}
