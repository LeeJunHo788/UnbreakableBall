using UnityEngine;

[CreateAssetMenu(menuName = "Augments/Effects/Upgrade/Split_UpgradeBallCount")]
public class Split_UpgradeBallCount : ScriptableObject, IAugmentEffect
{
		public void Apply(AugmentRuntimeContext ctx)
		{
				ctx.manager.splitAM.UpgradeSplitCount(1);
		}
}
