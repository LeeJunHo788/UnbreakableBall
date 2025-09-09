using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Augments/Augment Definition")]
public class AugmentDefinition : ScriptableObject
{
		[Header("표시 정보")]
		public string id;
		public string displayName;
		[TextArea] public string description;
		public Sprite icon;

		[Header("출현 조건(AND)")]
		public List<ScriptableObject> allConditions;

		[Header("효과")]
		public ScriptableObject effectSO;

		public bool IsEligible(in AugmentCheckContext ctx)
		{
				if (allConditions != null)
				{
						foreach (var so in allConditions)
						{
								if (so is IAugmentCondition cond)
								{
										if (!cond.Evaluate(ctx)) return false;
								}
						}
				}
				return true;
		}

		public IAugmentEffect GetEffect() => effectSO as IAugmentEffect;
}