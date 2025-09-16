using UnityEngine;

[CreateAssetMenu(menuName = "Augments/Conditions/HasNotAugmentCondition")]
public class HasNotAugmentCondition : ScriptableObject, IAugmentCondition
{
		[Tooltip("���� ��ȣ")]
		public string requiredAugmentId;

		public bool Evaluate(in AugmentCheckContext ctx)
		{
				return !ctx.OwnedAugmentIds.Contains(requiredAugmentId);
		}
}