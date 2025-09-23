using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpreadAugmentManager : MonoBehaviour
{
		private bool isActive;
		private PlayerController pc;
		private GameObject player;

		private bool isSpreadActive = false;

  [HideInInspector] public float interval = 1f;
  [HideInInspector] public int spreadNum = 4;
  [HideInInspector] public float spreadAtt = 0.1f;

		private List<GameObject> spreadObjects = new List<GameObject>();
		[Header("할당 오브젝트")]
		public GameObject spreadObject;


		public void Activate(AugmentRuntimeContext ctx)
		{
				pc = ctx.Player.GetComponent<PlayerController>();
				player = pc.gameObject;

				if (!isActive)
				{
						pc.OnPlayerFire += HandleSpreadActiveOn;
						pc.OnPlayerReady += HandleSpreadActiveOff;
						isActive = true;
				}
		}

		public void Deactivate()
		{
				if (isActive)
				{
						pc.OnPlayerFire -= HandleSpreadActiveOn;
						pc.OnPlayerReady -= HandleSpreadActiveOff;
						isActive = false;
				}
		}

		IEnumerator SpreadFireRoutine()
		{
				while(isSpreadActive)
				{
						yield return new WaitForSeconds(interval);

						if (pc.isMainBallDown) yield break;

						Vector3 pos = player.transform.position;

						for (int i = 0; i < spreadNum; i++)
						{
								float angle = (360f / spreadNum) * i;

								Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

								GameObject bullet = Instantiate(spreadObject, pos, Quaternion.identity);
        var col = bullet.GetComponent<Collider2D>();
        if (col) col.enabled = false;
								spreadObjects.Add(bullet);

        StartCoroutine(EnableColliderNextFixed(col));

        SpreadObjectController soc = bullet.GetComponent<SpreadObjectController>();
								Debug.Log(pc.ps.att);
								soc.Init(spreadAtt * pc.ps.att);

								Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
								rb.linearVelocity = dir * 10f;

						}

				}
		}

  private IEnumerator EnableColliderNextFixed(Collider2D c)
  {
    yield return new WaitForFixedUpdate();           
    yield return new WaitForFixedUpdate();           
    if (c) c.enabled = true;                         
  }

  public void HandleSpreadActiveOn()
		{
				isSpreadActive = true;
				StartCoroutine(SpreadFireRoutine());
		}

		public void HandleSpreadActiveOff()
		{
				isSpreadActive = false;

				for (int i = 0; i < spreadObjects.Count; i++)
				{
						Destroy(spreadObjects[i]);
				}
				spreadObjects.Clear();
		}

}
