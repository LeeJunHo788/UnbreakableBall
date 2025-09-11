using UnityEngine;

[CreateAssetMenu(menuName = "Augments/Reactive/SplitOnBlockHit")]
public class SplitOnBlockHitReactive : ScriptableObject, IAugmentReactive
{
		[Header("���� �� ������ (Rigidbody2D �ʿ�)")]
		public GameObject subBallPrefab;

		[Header("���� ������ �ߵ�")]
		public bool onlyMainBall = true;

		[Header("���� �л�")]
		public bool useAroundCurrentVelocity = true;
		[Range(0f, 180f)] public float spreadDegrees = 45f;

		[Header("���� ��ġ ������")]
		public Vector2 spawnOffset = Vector2.zero;

		public void Bind(AugmentManager manager, in AugmentRuntimeContext ctx)
		{
				manager.OnBlockHit += HandleOnBlockHit;
		}

		public void Unbind(AugmentManager manager)
		{
				manager.OnBlockHit -= HandleOnBlockHit;
		}

		private void HandleOnBlockHit(BallHitContext ctx)
		{
				if (onlyMainBall && !ctx.isMainBall) return;
				if (subBallPrefab == null) return;

				Vector3 spawnPos = (Vector3)ctx.hitPoint + (Vector3)spawnOffset;
				var sub = Instantiate(subBallPrefab, spawnPos, Quaternion.identity);
				var rb = sub.GetComponent<Rigidbody2D>();

				Vector2 dir;
				if (useAroundCurrentVelocity)
				{
						float baseDeg = Mathf.Atan2(ctx.ballVelocity.y, ctx.ballVelocity.x) * Mathf.Rad2Deg;
						float delta = Random.Range(-spreadDegrees, spreadDegrees);
						float finalDeg = baseDeg + delta;
						dir = new Vector2(Mathf.Cos(finalDeg * Mathf.Deg2Rad), Mathf.Sin(finalDeg * Mathf.Deg2Rad)).normalized;
				}
				else
				{
						float ang = Random.Range(0f, 360f);
						dir = new Vector2(Mathf.Cos(ang * Mathf.Deg2Rad), Mathf.Sin(ang * Mathf.Deg2Rad)).normalized;
				}

				rb.linearVelocity = dir * ctx.stats.moveSpeed;
		}
}
