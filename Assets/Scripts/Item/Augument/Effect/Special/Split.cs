using UnityEngine;

[CreateAssetMenu(menuName = "Augments/Reactive/Split")]
public class Split : ScriptableObject, IAugmentReactive
{
		public void Bind(AugmentManager manager, in AugmentRuntimeContext ctx)
		{
				manager.splitAM.Activate(this, in ctx); 
		}

		public void Unbind(AugmentManager manager)
		{
				manager.splitAM.Deactivate();
		}

		[Header("서브 공")]
		public GameObject subBallPrefab;

		[Header("메인 공에만 발동")]
		public bool onlyMainBall = true;

		[Header("분산 각도")]
		[Range(0f, 180f)] public float spreadDegrees = 45f;

		[Header("스폰 위치 오프셋")]
		public Vector2 spawnOffset = Vector2.zero;

}