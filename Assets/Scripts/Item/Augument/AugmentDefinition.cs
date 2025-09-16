using UnityEngine;
using System.Collections.Generic;

public enum AugmentKind
{
  Normal,
		Upgrade,
  Special
}

[CreateAssetMenu(menuName = "Augments/Augment Definition")]
public class AugmentDefinition : ScriptableObject
{
		[Header("증강 종류")]
		public AugmentKind augmentKind = AugmentKind.Normal;
		[Header("증강 번호")]
		public string id;
		[Header("증강 등급")]
		[Range(1,5)]public int grade;
		[Header("증강 이름")]
		public string displayName;
		[Header("증강 설명")]
		[TextArea] public string description;
		[Header("증강 아이콘")]
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