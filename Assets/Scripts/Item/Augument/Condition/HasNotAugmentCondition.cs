using UnityEngine;

[CreateAssetMenu(menuName = "Augments/Conditions/HasNotAugmentCondition")]
public class HasNotAugmentCondition : ScriptableObject, IAugmentCondition
{
		[Tooltip("증강 번호")]
		public string requiredAugmentId;

		public bool Evaluate(in AugmentCheckContext ctx)
		{
				return !ctx.OwnedAugmentIds.Contains(requiredAugmentId);
		}
}