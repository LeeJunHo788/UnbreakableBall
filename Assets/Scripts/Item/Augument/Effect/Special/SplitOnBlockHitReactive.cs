using UnityEngine;

[CreateAssetMenu(menuName = "Augments/Reactive/SplitOnBlockHit")]
public class SplitOnBlockHitReactive : ScriptableObject, IAugmentReactive
{
		[Header("���� �� ������ (Rigidbody2D �ʿ�)")]
		public GameObject subBallPrefab;

		[Header("���� ������ �ߵ�")]
		public bool onlyMainBall = true;

		[Header("�л� ����")]
		[Range(0f, 180f)] public float spreadDegrees = 45f;

		[Header("���� ��ġ ������")]
		public Vector2 spawnOffset = Vector2.zero;

		public void Bind(AugmentManager manager, in AugmentRuntimeContext ctx)
		{
				manager.splitAugmentManager.Activate(this, in ctx); 
		}

		public void Unbind(AugmentManager manager)
		{
				manager.splitAugmentManager.Deactivate();
		}
}