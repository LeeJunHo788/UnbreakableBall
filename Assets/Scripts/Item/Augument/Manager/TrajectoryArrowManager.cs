using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Transform))]
public class TrajectoryArrowManager : MonoBehaviour
{
		private bool isActive = false;
		private PlayerController pc;
		private LineRenderer lr;

		private float lineWidth = 0.07f;
		private float segmentMaxDistance = 30f;
		public GameObject circleObj;

		[Header("반사 횟수")]
		public int bounceCount = 0;

  [Header("레이캐스트 마스크")]
  public LayerMask raycastMaskNormal;       
  public LayerMask raycastMaskWithDSideBar; 

  private bool isVisible = false;
		private readonly List<Vector3> points = new List<Vector3>();

		
		public void Activate(AugmentRuntimeContext ctx)
		{
				if(!isActive)
				{
						pc = PlayerController.Instance;
						lr = GetComponent<LineRenderer>();
						lr.startWidth = lr.endWidth = lineWidth;
						lr.enabled = false;

						circleObj = Instantiate(circleObj);
						circleObj.SetActive(false);

      // 이벤트 구독
      pc.OnPlayerFire += HandleFire;
      pc.OnPlayerReady += HandleReady;

      if (pc.isReady) Show();
      else Hide();

      isActive = true;

				}
  }

		public void Deactivate()
		{
				if(isActive)
				{
      pc.OnPlayerFire -= HandleFire;
      pc.OnPlayerReady -= HandleReady;
						Hide();

						isActive = false;
    }
    
  }

  public void Show()
  {
    isVisible = true;
    lr.enabled = true;
    circleObj.SetActive(true);
    UpdateLine();
    UpdateCircle();
  }

  public void Hide()
		{
				isVisible = false;
				lr.enabled = false;
				circleObj.SetActive(false);
				
		}

		private void LateUpdate()
		{
				if (isVisible && pc != null && pc.isReady)
				{
						UpdateLine();
      UpdateCircle();

    }
		}

		private void UpdateLine()
		{
				if (lr == null || pc == null) return;

				Vector2 origin = pc.transform.position;

				float angleRad = pc.ps.angle * Mathf.Deg2Rad;
				Vector2 dir = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad)).normalized;

				points.Clear();
				points.Add(origin);


				Vector2 currentPos = origin;
				Vector2 currentDir = dir;
				int bounces = 0;

				while (bounces <= bounceCount)
				{
						LayerMask mask = (bounces == 0) ? raycastMaskNormal : raycastMaskWithDSideBar;

      RaycastHit2D hit = Physics2D.Raycast(currentPos, currentDir, segmentMaxDistance, mask);

						if (hit.collider != null)
						{
        points.Add(hit.point);

        if (hit.collider.CompareTag("DSideBar"))
          break;


								if (bounces < bounceCount)
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

		private void UpdateCircle()
		{
    Vector3 endPos = lr.GetPosition(lr.positionCount - 1);
				circleObj.transform.position = endPos;
  }

  private void HandleFire() => Hide();
  private void HandleReady() => Show();
}