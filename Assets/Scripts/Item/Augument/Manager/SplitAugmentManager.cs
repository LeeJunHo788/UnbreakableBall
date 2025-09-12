using UnityEngine;

public class SplitAugmentManager : MonoBehaviour
{
		[HideInInspector] public int tempAddedThisRound = 0;

		private AugmentManager augmentManager;    
		private PlayerStats playerStats; 
		private bool isActive = false;   

		// 설정값 보관
		private GameObject subBallPrefab;
		private bool onlyMainBall;
		private float spreadDegrees;
		private Vector2 spawnOffset;

		// 분열 개수
		private int splitCount = 1;
		private int bonusSplitCount = 0;

		public void Init(AugmentManager owner, PlayerStats stats)
		{
				augmentManager = owner;
				playerStats = stats;
		}

		public void Activate(SplitOnBlockHitReactive so, in AugmentRuntimeContext ctx)
		{
				subBallPrefab = so.subBallPrefab;
				onlyMainBall = so.onlyMainBall;
				spreadDegrees = so.spreadDegrees;
				spawnOffset = so.spawnOffset;

				if (!isActive)
				{
						augmentManager.OnBlockHit += HandleOnBlockHit;
						isActive = true;
				}
		}

		public void Deactivate()
		{
				if (isActive)
				{
						augmentManager.OnBlockHit -= HandleOnBlockHit;
						isActive = false;
				}
				bonusSplitCount = 0;
		}

		public void UpgradeSplitCount(int amount = 1)
		{
				bonusSplitCount += amount;
		}

		private void HandleOnBlockHit(BallHitContext ctx)
		{
				if (onlyMainBall && !ctx.isMainBall) return;
				if (subBallPrefab == null || playerStats == null) return;

				int totalSplit = splitCount + bonusSplitCount;

				for (int i = 0; i < totalSplit; i++)
				{
						Vector3 spawnPos = (Vector3)ctx.hitPoint + (Vector3)spawnOffset;

						var sub = Instantiate(subBallPrefab, spawnPos, Quaternion.identity);
						var rb = sub.GetComponent<Rigidbody2D>();

						var stats = sub.GetComponent<PlayerStats>();
						if (stats != null)
								stats.att = stats.att / 4;

						Vector2 dir = SetDirection(ctx);
						while (Mathf.Abs(Vector2.Dot(dir, Vector2.right)) > 0.98f)
								dir = SetDirection(ctx);

						rb.linearVelocity = dir * ctx.stats.moveSpeed;

						RegisterTempAdditionalBall();
						PlayerController.Instance.RegisterSubBall(sub);
				}
		}

		private Vector2 SetDirection(BallHitContext ctx)
		{
				float baseDeg = Mathf.Atan2(ctx.ballVelocity.y, ctx.ballVelocity.x) * Mathf.Rad2Deg;
				float delta = Random.Range(-spreadDegrees, spreadDegrees);
				float finalDeg = baseDeg + delta;
				return new Vector2(Mathf.Cos(finalDeg * Mathf.Deg2Rad),
																							Mathf.Sin(finalDeg * Mathf.Deg2Rad)).normalized;
		}

		public void RegisterTempAdditionalBall()
		{
				tempAddedThisRound++;
				if (playerStats != null) playerStats.additionalBallCount++;
		}
}
