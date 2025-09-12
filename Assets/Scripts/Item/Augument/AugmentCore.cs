using System.Collections.Generic;
using UnityEngine;

// ���� ���� ���� �򰡿� �ʿ��� ��
public struct AugmentCheckContext
{
		public int CurrentLevel;		// ���� ����
		public HashSet<string> OwnedAugmentIds;	// ������ �ִ� ���� ���̵�
																																													
}

// ���� ȿ�� ���� �� �ʿ��� ������
public struct AugmentRuntimeContext
{
		public GameObject Player;
		public PlayerStats Stats;
		public AugmentManager manager;
}

// ���� �������̽� (���� SO�� ����)
public interface IAugmentCondition
{
		bool Evaluate(in AugmentCheckContext ctx);
}

// ȿ�� �������̽� (���� SO�� ����)
public interface IAugmentEffect
{
		void Apply(AugmentRuntimeContext ctx);
}

// ��Ÿ�� �̺�Ʈ�� �����ϴ� ������ �������̽�
public interface IAugmentReactive
{
		// �̺�Ʈ ���� ���
		void Bind(AugmentManager manager, in AugmentRuntimeContext ctx);

		// �̺�Ʈ ���� ����
		void Unbind(AugmentManager manager);
}

// ���� �ε����� ���޹��� �� ���� ����ü
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