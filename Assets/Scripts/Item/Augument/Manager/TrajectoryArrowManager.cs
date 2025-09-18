// [추가] 조준 궤적을 그려주는 전담 컴포넌트 (플레이어에 붙음)
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Transform))]
public class TrajectoryArrowManager : MonoBehaviour
{
		private PlayerController pc;
		private LineRenderer lr;

		[Header("라인 세팅")]
		public float lineWidth = 0.06f;
		public float segmentMaxDistance = 30f;
		public LayerMask raycastMask;

		[Header("반사 관련")]
		[Range(0, 5)] public int maxBounceCount = 0;  // 직접 들고 있는 반사 횟수

		private bool isVisible = false;
		private readonly List<Vector3> points = new List<Vector3>();

		private void Awake()
		{
				pc = PlayerController.Instance;
				lr = GetComponent<LineRenderer>() ?? gameObject.AddComponent<LineRenderer>();
				lr.startWidth = lr.endWidth = lineWidth;
				lr.enabled = false;
		}

		public void Show()
		{
				isVisible = true;
				lr.enabled = true;
				UpdateLine();
		}

		public void Hide()
		{
				isVisible = false;
				lr.enabled = false;
				lr.positionCount = 0;
		}

		private void LateUpdate()
		{
				if (isVisible && pc != null && pc.isReady)
						UpdateLine();
		}

		private void UpdateLine()
		{
				if (lr == null || pc == null) return;

				// 플레이어 발사 각도 가져오기
				float angleRad = pc.ps.angle * Mathf.Deg2Rad;
				Vector2 dir = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad)).normalized;
				Vector2 origin = pc.transform.position;

				points.Clear();
				points.Add(origin);

				Vector2 currentPos = origin;
				Vector2 currentDir = dir;

				int bounces = 0;
				while (bounces <= maxBounceCount)
				{
						RaycastHit2D hit = Physics2D.Raycast(currentPos, currentDir, segmentMaxDistance, raycastMask);

						if (hit.collider != null)
						{
								points.Add(hit.point);

								if (bounces < maxBounceCount)
								{
										Vector2 reflected = Vector2.Reflect(currentDir, hit.normal).normalized;
										currentPos = hit.point + reflected * 0.01f;
										currentDir = reflected;
										bounces++;
								}
								else break;
						}
						else
						{
								points.Add(currentPos + currentDir * segmentMaxDistance);
								break;
						}
				}

				lr.positionCount = points.Count;
				for (int i = 0; i < points.Count; i++)
						lr.SetPosition(i, points[i]);
		}
}