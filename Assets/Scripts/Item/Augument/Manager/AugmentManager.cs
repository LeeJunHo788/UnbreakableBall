using UnityEngine;
using System.Collections.Generic;

public class AugmentManager : MonoBehaviour
{
		[Header("전체 증강 목록")]
		public List<AugmentDefinition> allAugments = new List<AugmentDefinition>();

		[Header("보유한 증강 ID")]
		public HashSet<string> owned = new HashSet<string>(); // 런타임

		// 보유한 반응형 증강
		private List<IAugmentReactive> activeReactives = new List<IAugmentReactive>();

  [Header("등장 가중치")]
  public float grade1Weight = 1f; 
  public float grade2Weight = 2f;  
  public float grade3Weight = 3f;  
  public float grade4Weight = 4f;

  [Header("UI 연결")]
		public AugmentChoiceUI choiceUI;

  [Header("특수증강")]
  public int specialMax = 3;                             
  [SerializeField] private SpecialAugmentBar specialBar; 

  private readonly List<string> ownedSpecialIds = new List<string>();
  public int SpecialCount => ownedSpecialIds.Count;


  [Header("플레이어 참조")]
		public GameObject player;
		public PlayerStats playerStats;

		[Header("증강 매니저")]
		public SplitAugmentManager splitAM;
		public CanonAugmentManager canonAM;
		public SpreadAugmentManager spreadAM;


		public event System.Action OnAugmentFinished;

		private void Awake()
		{
				if (splitAM != null)
      splitAM.Init(this, playerStats); // <-- [추가]
		}

		// ============이벤트 허브=============
		public event System.Action<BallHitContext> OnBlockHit;


		// ============이벤트 허브=============

		public void AugmentApply(int currentLevel)
		{
				// 조건 검사에 필요한 값 구조체 인스턴스 생성
				var ctx = new AugmentCheckContext
				{
						CurrentLevel = currentLevel,
						OwnedAugmentIds = owned
				};

				// 조건 필터링
				List<AugmentDefinition> candidates = new List<AugmentDefinition>();
    bool canOfferSpecial = SpecialCount < specialMax;


    foreach (var aug in allAugments)
				{
						// 특수 증강 체크
      if (!canOfferSpecial && aug.augmentKind == AugmentKind.Special)
        continue;

      // 기본 등장조건
      if (aug.IsEligible(ctx) && (aug.AllowDuplicate || !owned.Contains(aug.id)))
      {
        candidates.Add(aug);
      }
    }

				// 후보가 없으면 즉시 종료
    if (candidates.Count == 0)
    {
      OnAugmentFinished?.Invoke(); 
      return;
    }
				
				// 가중치에 의한 선택
    var shown = WeightPick(candidates, 3);

    // UI 표시
    choiceUI.Show(shown, OnPickAugment);
		}

		// 증강 선택시 호출
		private void OnPickAugment(AugmentDefinition picked)
		{
				// 구조체 인스턴스 생성
				var runtime = new AugmentRuntimeContext
				{
						Player = player,
						Stats = playerStats,
						manager = this
				};

				// 효과 적용
				picked.GetEffect()?.Apply(runtime);   // 1회성 증강
				
				// 반응형 증강
				var reactive = picked.GetReactive();
				if (reactive != null)
				{
						reactive.Bind(this, in runtime);
						activeReactives.Add(reactive);   // 보관
				}

				owned.Add(picked.id);   // 보유 증강 리스트에 추가

    if (picked.augmentKind == AugmentKind.Special)
    {
      if (!ownedSpecialIds.Contains(picked.id))
      {
        ownedSpecialIds.Add(picked.id);																																																									
        if (specialBar) specialBar.AddIcon(picked.icon);    
      }
    }

    // 재개
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

		// 특정 타입의 반응형 증강 찾기
		public T GetReactive<T>() where T : class, IAugmentReactive
		{
				foreach (var r in activeReactives)
				{
						if (r is T typed) return typed;
				}
				return null;
		}


		// ========= 반응형 증강에 필요한 이벤트 바인딩 함수 ==============

		public void RaiseBlockHit(in BallHitContext ctx)
		{
				var context = ctx;
				context.stats = playerStats;
				OnBlockHit?.Invoke(context);
		}

		
}