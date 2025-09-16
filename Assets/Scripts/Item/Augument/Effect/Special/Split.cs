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

		[Header("���� ��")]
		public GameObject subBallPrefab;

		[Header("���� ������ �ߵ�")]
		public bool onlyMainBall = true;

		[Header("�л� ����")]
		[Range(0f, 180f)] public float spreadDegrees = 45f;

		[Header("���� ��ġ ������")]
		public Vector2 spawnOffset = Vector2.zero;

}