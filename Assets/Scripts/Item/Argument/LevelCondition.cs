using UnityEngine;

[CreateAssetMenu(menuName = "Augments/Conditions/LevelReached")]
public class LevelCondition : ScriptableObject, IAugmentCondition
{
		public int requiredLevel = 5;

		public bool Evaluate(in AugmentCheckContext ctx)
		{
				return ctx.CurrentLevel >= requiredLevel;
		}
}