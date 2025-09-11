using UnityEngine;

[CreateAssetMenu(menuName = "Augments/Conditions/HasAugmentCondition")]
public class HasAugmentCondition : ScriptableObject, IAugmentCondition
{
		[Tooltip("가지고 있어야 할 증강 번호")]
		public string requiredAugmentId;

		public bool Evaluate(in AugmentCheckContext ctx)
		{
				return ctx.OwnedAugmentIds.Contains(requiredAugmentId);
		}
}