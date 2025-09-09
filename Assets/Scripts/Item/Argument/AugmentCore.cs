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