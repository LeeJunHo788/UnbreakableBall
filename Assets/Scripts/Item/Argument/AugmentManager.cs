using UnityEngine;
using System.Collections.Generic;

public class AugmentManager : MonoBehaviour
{
		[Header("��ü ���� ��� (SO ���µ�)")]
		public List<AugmentDefinition> allAugments = new List<AugmentDefinition>();

		[Header("������ ���� ID (��Ÿ��)")]
		public HashSet<string> owned = new HashSet<string>();

		[Header("UI ����")]
		public AugmentChoiceUI choiceUI;

		[Header("�÷��̾� ����")]
		public GameObject player;
		public PlayerStats playerStats;

		public event System.Action OnAugmentFinished;


  public void AugmentApply(int currentLevel)
		{
				// ���� �˻翡 �ʿ��� �� ����ü �ν��Ͻ� ����
				var ctx = new AugmentCheckContext
				{
						CurrentLevel = currentLevel,
						OwnedAugmentIds = owned
				};

				// ���� ���͸�
				List<AugmentDefinition> candidates = new List<AugmentDefinition>();
				foreach (var aug in allAugments)
				{
      // ���� ���ǿ� �����ϸ� ����
      if (aug.IsEligible(ctx) && (aug.AllowDuplicate || !owned.Contains(aug.id)))
      {
        candidates.Add(aug);
      }
    }

				// ����
				Shuffle(candidates); 
				var shown = candidates.Count > 3 ? candidates.GetRange(0, 3) : candidates;

				// �ĺ��� ������ ��� ����
    if (shown.Count == 0)
    {
      OnAugmentFinished?.Invoke(); 
      return;
    }

    // UI ǥ��
				choiceUI.Show(shown, OnPickAugment);
		}

		// ���� ���ý� ȣ��
		private void OnPickAugment(AugmentDefinition picked)
		{
				// ����ü �ν��Ͻ� ����
				var runtime = new AugmentRuntimeContext
				{
						Player = player,
						Stats = playerStats
				};

				// ȿ�� ����
				picked.GetEffect()?.Apply(runtime);
				owned.Add(picked.id);

    // �簳
    choiceUI.Hide(() =>                       
    {
      OnAugmentFinished?.Invoke();            
    });
  }

		// ����
		private void Shuffle<T>(IList<T> list)
		{
				for (int i = 0; i < list.Count; i++)
				{
						int j = Random.Range(i, list.Count);
						(list[i], list[j]) = (list[j], list[i]);
				}
		}
}