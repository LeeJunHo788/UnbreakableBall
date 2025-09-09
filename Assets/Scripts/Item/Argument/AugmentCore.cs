using System.Collections.Generic;
using UnityEngine;

// 증강 등장 조건 평가에 필요한 값
public struct AugmentCheckContext
{
		public int CurrentLevel;		// 현재 레벨
		public HashSet<string> OwnedAugmentIds;	// 가지고 있는 증강 아이디
																																													
}

// 증강 효과 적용 시 필요한 참조들
public struct AugmentRuntimeContext
{
		public GameObject Player;
		public PlayerStats Stats;
}

// 조건 인터페이스 (하위 SO가 구현)
public interface IAugmentCondition
{
		bool Evaluate(in AugmentCheckContext ctx);
}

// 효과 인터페이스 (하위 SO가 구현)
public interface IAugmentEffect
{
		void Apply(AugmentRuntimeContext ctx);
}