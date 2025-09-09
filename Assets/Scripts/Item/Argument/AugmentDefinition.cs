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
		[Header("ǥ�� ����")]
		public AugmentKind augmentKind = AugmentKind.Normal;
		public string id;
		public string displayName;
		[TextArea] public string description;
		public Sprite icon;

		[HideInInspector]
		public bool AllowDuplicate => augmentKind == AugmentKind.Normal;

		[Header("���� ����(AND)")]
		public List<ScriptableObject> allConditions;

		[Header("ȿ��")]
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