using UnityEngine;

public class Item : MonoBehaviour
{

		[HideInInspector] public float downSpeed = 3.5f;
		[HideInInspector] public float moveSpeed = 10f;
		[HideInInspector] public bool isDroping = true;
		[HideInInspector] public int dropWeight = 1;

		protected void Update()
		{
				if (isDroping)
						MoveToDown();
				else
						MoveToPlayer();
		}
		
		private void MoveToDown()
		{
				transform.position = new Vector3(transform.position.x, transform.position.y - (Time.deltaTime * downSpeed));
		}

		private void MoveToPlayer()
		{
				Vector3 playerPos = PlayerController.Instance.gameObject.transform.position;
				Vector3 dir = (playerPos - transform.position).normalized;
				transform.position += dir * moveSpeed;
		}

		protected virtual void Apply(PlayerController pc)
		{

		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
				if (collision.CompareTag("Player"))
				{
						PlayerController pc = collision.GetComponent<PlayerController>();
						if (pc != null)
								Apply(pc);
						Destroy(gameObject);
				}

				else if (collision.CompareTag("DSideBar"))
				{
						Destroy(gameObject);
				}
		}


}
