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
		[Header("���� ����")]
		public AugmentKind augmentKind = AugmentKind.Normal;
		[Header("���� ��ȣ")]
		public string id;
		[Header("���� ���")]
		[Range(1,5)]public int grade;
		[Header("���� �̸�")]
		public string displayName;
		[Header("���� ����")]
		[TextArea] public string description;
		[Header("���� ������")]
		public Sprite icon;

		[HideInInspector]
		public bool AllowDuplicate => augmentKind == AugmentKind.Normal;

		[Header("���� ����(AND)")]
		public List<ScriptableObject> allConditions;

		[Header("1ȸ�� ȿ��")]
		public ScriptableObject effectSO;
		[Header("������ ȿ��")]
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