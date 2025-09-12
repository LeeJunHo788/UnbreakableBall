using UnityEngine;

[CreateAssetMenu(menuName = "Augments/Reactive/SplitOnBlockHit")]
public class SplitOnBlockHitReactive : ScriptableObject, IAugmentReactive
{
		[Header("서브 공 프리팹 (Rigidbody2D 필요)")]
		public GameObject subBallPrefab;

		[Header("메인 공에만 발동")]
		public bool onlyMainBall = true;

		[Header("분산 각도")]
		[Range(0f, 180f)] public float spreadDegrees = 45f;

		[Header("스폰 위치 오프셋")]
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