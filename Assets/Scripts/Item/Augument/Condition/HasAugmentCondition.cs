using UnityEngine;

[CreateAssetMenu(menuName = "Augments/Conditions/HasAugmentCondition")]
public class HasAugmentCondition : ScriptableObject, IAugmentCondition
{
		[Tooltip("���� ��ȣ")]
		public string requiredAugmentId;

		public bool Evaluate(in AugmentCheckContext ctx)
		{
				return ctx.OwnedAugmentIds.Contains(requiredAugmentId);
		}
}