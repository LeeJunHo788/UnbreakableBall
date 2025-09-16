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

  [Header("���� ����ġ")]
  public float grade1Weight = 1f; 
  public float grade2Weight = 2f;  
  public float grade3Weight = 3f;  
  public float grade4Weight = 4f;

  [Header("UI ����")]
		public AugmentChoiceUI choiceUI;

  [Header("Ư������")]
  public int specialMax = 3;                             
  [SerializeField] private SpecialAugmentBar specialBar; 

  private readonly List<string> ownedSpecialIds = new List<string>();
  public int SpecialCount => ownedSpecialIds.Count;


  [Header("�÷��̾� ����")]
		public GameObject player;
		public PlayerStats playerStats;

		[Header("���� �Ŵ���")]
		public SplitAugmentManager splitAM;
		public CanonAugmentManager canonAM;
		public SpreadAugmentManager spreadAM;


		public event System.Action OnAugmentFinished;

		private void Awake()
		{
				if (splitAM != null)
      splitAM.Init(this, playerStats); // <-- [�߰�]
		}

		// ============�̺�Ʈ ���=============
		public event System.Action<BallHitContext> OnBlockHit;


		// ============�̺�Ʈ ���=============

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
    bool canOfferSpecial = SpecialCount < specialMax;


    foreach (var aug in allAugments)
				{
						// Ư�� ���� üũ
      if (!canOfferSpecial && aug.augmentKind == AugmentKind.Special)
        continue;

      // �⺻ ��������
      if (aug.IsEligible(ctx) && (aug.AllowDuplicate || !owned.Contains(aug.id)))
      {
        candidates.Add(aug);
      }
    }

				// �ĺ��� ������ ��� ����
    if (candidates.Count == 0)
    {
      OnAugmentFinished?.Invoke(); 
      return;
    }
				
				// ����ġ�� ���� ����
    var shown = WeightPick(candidates, 3);

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

				owned.Add(picked.id);   // ���� ���� ����Ʈ�� �߰�

    if (picked.augmentKind == AugmentKind.Special)
    {
      if (!ownedSpecialIds.Contains(picked.id))
      {
        ownedSpecialIds.Add(picked.id);																																																									
        if (specialBar) specialBar.AddIcon(picked.icon);    
      }
    }

    // �簳
    choiceUI.Hide(() =>                       
    {
      OnAugmentFinished?.Invoke();            
    });
  }

		private List<AugmentDefinition> WeightPick(List<AugmentDefinition> augmentList, int n)
		{
    n = Mathf.Clamp(n, 0, augmentList.Count);

    var pool = new List<AugmentDefinition>(augmentList);
    var result = new List<AugmentDefinition>(n);

    for (int pick = 0; pick < n; pick++)
    {
      float total = 0f;
      for (int i = 0; i < pool.Count; i++) total += GetWeightValue(pool[i]);

      float r = Random.value * total;
      float acc = 0f;
						int chosenIndex = -1;

      for (int i = 0; i < pool.Count; i++)
      {
        acc += GetWeightValue(pool[i]);
        if (r <= acc)
        {
          chosenIndex = i;
          break;
        }
      }

      result.Add(pool[chosenIndex]);
      pool.RemoveAt(chosenIndex);
    }

    return result;
  }

		private float GetWeightValue(AugmentDefinition aug)
		{
				switch (aug.grade)
				{
						case 1: return grade1Weight;
						case 2: return grade2Weight;
						case 3: return grade3Weight;
						case 4: return grade4Weight;
						default: return 1;
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