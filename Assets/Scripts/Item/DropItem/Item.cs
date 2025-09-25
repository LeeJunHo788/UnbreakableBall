using UnityEngine;

public class Item : MonoBehaviour
{

		private float downSpeed = 3.5f;
		private float moveSpeed = 10f;
		public bool isDroping = true;
		public int dropWeight = 1;

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
				transform.position += dir * Time.deltaTime * moveSpeed;
		}

		protected virtual void Apply(PlayerController pc)
		{
				Debug.Log("»£√‚");
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
