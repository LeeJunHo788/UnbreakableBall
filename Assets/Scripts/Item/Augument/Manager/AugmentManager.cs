using UnityEngine;
using System.Collections.Generic;

public class AugmentManager : MonoBehaviour
{
		[Header("��ü ���� ���")]
		public List<AugmentDefinition> allAugments = new List<AugmentDefinition>();

		[Header("������ ���� ID")]
		public HashSet<string> owned = new HashSet<string>(); // ��Ÿ��

		// ������ ������ ����
		private List<IAugmentReactive> activeReactives = new List<IAugmentReactive>();

		[Header("UI ����")]
		public AugmentChoiceUI choiceUI;

		[Header("�÷��̾� ����")]
		public GameObject player;
		public PlayerStats playerStats;

		[Header("���� �Ŵ���")]
		public SplitAugmentManager splitAugmentManager;


		public event System.Action OnAugmentFinished;

		private void Awake()
		{
				if (splitAugmentManager != null)
						splitAugmentManager.Init(this, playerStats); // <-- [�߰�]
		}


		// ============�̺�Ʈ ���=============
		public event System.Action<BallHitContext> OnBlockHit;


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
						Stats = playerStats,
						manager = this
				};

				// ȿ�� ����
				picked.GetEffect()?.Apply(runtime);   // 1ȸ�� ����
				
				// ������ ����
				var reactive = picked.GetReactive();
				if (reactive != null)
				{
						reactive.Bind(this, in runtime);
						activeReactives.Add(reactive);   // ����
				}

				owned.Add(picked.id);			// ���� ���� ����Ʈ�� �߰�

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

		// Ư�� Ÿ���� ������ ���� ã��
		public T GetReactive<T>() where T : class, IAugmentReactive
		{
				foreach (var r in activeReactives)
				{
						if (r is T typed) return typed;
				}
				return null;
		}


		// ========= ������ ������ �ʿ��� �̺�Ʈ ���ε� �Լ� ==============

		public void RaiseBlockHit(in BallHitContext ctx)
		{
				var context = ctx;
				context.stats = playerStats;
				OnBlockHit?.Invoke(context);
		}

		
}