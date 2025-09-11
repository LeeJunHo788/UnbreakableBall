using UnityEngine;
using System.Collections.Generic;

public enum AugmentKind
{
  Normal,
  Special
}

[CreateAssetMenu(menuName = "Augments/Augment Definition")]
public class AugmentDefinition : ScriptableObject
{
		[Header("표시 정보")]
		public AugmentKind augmentKind = AugmentKind.Normal;
		public string id;
		public string displayName;
		[TextArea] public string description;
		public Sprite icon;

		[HideInInspector]
		public bool AllowDuplicate => augmentKind == AugmentKind.Normal;

		[Header("출현 조건(AND)")]
		public List<ScriptableObject> allConditions;

		[Header("1회성 효과")]
		public ScriptableObject effectSO;
		[Header("반응형 효과")]
		public ScriptableObject reactive;

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

		public IAugmentReactive GetReactive() => reactive as IAugmentReactive;
}