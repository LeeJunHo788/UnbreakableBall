using UnityEngine;

[CreateAssetMenu(menuName = "Augments/Effects/UpgradeSplitBall")]
public class UpgradeSplitBallEffect : ScriptableObject, IAugmentEffect
{
		public void Apply(AugmentRuntimeContext ctx)
		{
				ctx.manager.splitAugmentManager.UpgradeSplitCount(1);
		}
}
