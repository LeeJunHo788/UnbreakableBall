using UnityEngine;
using System.Collections.Generic;

public class AugmentManager : MonoBehaviour
{
		[Header("전체 증강 목록")]
		public List<AugmentDefinition> allAugments = new List<AugmentDefinition>();

		[Header("보유한 증강 ID")]
		public HashSet<string> owned = new HashSet<string>();	// 런타임

		[Header("UI 연결")]
		public AugmentChoiceUI choiceUI;

		[Header("플레이어 참조")]
		public GameObject player;
		public PlayerStats playerStats;

		public event System.Action OnAugmentFinished;

		// ============이벤트 허브=============
		public event System.Action<BallHitContext> OnBlockHit;


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
				foreach (var aug in allAugments)
				{
      // 등장 조건에 만족하면 등장
      if (aug.IsEligible(ctx) && (aug.AllowDuplicate || !owned.Contains(aug.id)))
      {
        candidates.Add(aug);
      }
    }

				// 셔플
				Shuffle(candidates); 
				var shown = candidates.Count > 3 ? candidates.GetRange(0, 3) : candidates;

				// 후보가 없으면 즉시 종료
    if (shown.Count == 0)
    {
      OnAugmentFinished?.Invoke(); 
      return;
    }

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
						Stats = playerStats
				};

				// 효과 적용
				picked.GetEffect()?.Apply(runtime);   // 1회성 증강


				var reactive = picked.GetReactive(); // 반응형 증강
				if (reactive != null)
				{
						reactive.Bind(this, in runtime);
				}

				owned.Add(picked.id);			// 보유 증강 리스트에 추가

    // 재개
    choiceUI.Hide(() =>                       
    {
      OnAugmentFinished?.Invoke();            
    });
  }

		// 셔플
		private void Shuffle<T>(IList<T> list)
		{
				for (int i = 0; i < list.Count; i++)
				{
						int j = Random.Range(i, list.Count);
						(list[i], list[j]) = (list[j], list[i]);
				}
		}


		// ========= 반응형 증강에 필요한 이벤트 바인딩 함수 ==============

		public void RaiseBrickHit(in BallHitContext ctx)
		{
				var context = ctx;
				context.stats = playerStats;
				OnBlockHit?.Invoke(context);
		}
}