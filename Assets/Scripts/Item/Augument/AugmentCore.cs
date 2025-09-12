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
		public AugmentManager manager;
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

// 런타임 이벤트에 반응하는 증강용 인터페이스
public interface IAugmentReactive
{
		// 이벤트 구독 등록
		void Bind(AugmentManager manager, in AugmentRuntimeContext ctx);

		// 이벤트 구독 해제
		void Unbind(AugmentManager manager);
}

// 공이 부딪힐때 전달받을 공 정보 구조체
public struct BallHitContext
{
		public GameObject ball;
		public Rigidbody2D ballRb;
		public bool isMainBall;
		public GameObject block;
		public Vector2 hitPoint;
		public Vector2 ballVelocity;
		public PlayerStats stats;
}