using UnityEngine;
using System.Collections;

public class SpreadAugmentManager : MonoBehaviour
{
		private bool isActive;
		private PlayerController pc;
		private GameObject player;

		private bool isSpreadActive = false;

		public float interval = 3;
		public int spreadNum = 4;

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

						Vector3 pos = player.transform.position;

						for (int i = 0; i < spreadNum; i++)
						{
								float angle = (360f / spreadNum) * i;

								Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

								GameObject bullet = Instantiate(spreadObject, dir, Quaternion.identity);

								Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
								rb.linearVelocity = dir * 5f;

						}

				}
		}

		public void HandleSpreadActiveOn()
		{
				isSpreadActive = true;
				StartCoroutine(SpreadFireRoutine());
		}

		public void HandleSpreadActiveOff()
		{
				isSpreadActive = false;
		}

}
