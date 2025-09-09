using UnityEngine;
using System.Collections.Generic;

public class AugmentManager : MonoBehaviour
{
		[Header("전체 증강 목록 (SO 에셋들)")]
		public List<AugmentDefinition> allAugments = new List<AugmentDefinition>();

		[Header("보유한 증강 ID (런타임)")]
		public HashSet<string> owned = new HashSet<string>();

		[Header("UI 연결")]
		public AugmentChoiceUI choiceUI;

		[Header("플레이어 참조")]
		public GameObject player;
		public PlayerStats playerStats;

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
						// 등장 조건에 만족하면서 증강을 가지고 있지 않을 경우 등장
						if (aug.IsEligible(ctx) && !owned.Contains(aug.id)) 
								candidates.Add(aug);
				}

				// 셔플
				Shuffle(candidates); 
				var shown = candidates.Count > 3 ? candidates.GetRange(0, 3) : candidates;

				// 게임 일시정지 UI 표시
				Time.timeScale = 0f; //  UI는 UnscaledTime으로 동작하도록 설정
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
				picked.GetEffect()?.Apply(runtime);
				owned.Add(picked.id);

				// 재개
				choiceUI.Hide();
				Time.timeScale = 1f;
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
}