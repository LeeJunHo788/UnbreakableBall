using UnityEngine;

[CreateAssetMenu(menuName = "Augments/Conditions/OverLevelCondition")]
public class OverLevelCondition : ScriptableObject, IAugmentCondition
{
		public int requiredLevel = 1;

		public bool Evaluate(in AugmentCheckContext ctx)
		{
				return ctx.CurrentLevel >= requiredLevel;
		}
}