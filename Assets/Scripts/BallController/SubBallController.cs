using UnityEngine;
using DG.Tweening;

public class SubBallController : MonoBehaviour
{
  public bool isFirstDown = false;

		public PlayerStats ss;

  private void Start()
  {
				ss = PlayerController.Instance.ps;
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.collider.CompareTag("DSideBar"))
    {
      float speed = 20;

      if (!PlayerController.Instance.isStartPosFixed)
      {
        PlayerController.Instance.startPos = new Vector3(transform.position.x, -6.57f);
        PlayerController.Instance.isStartPosFixed = true;
								PlayerController.Instance.canForceReady = true;
								isFirstDown = true;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.zero;
      }


      float distance = Vector3.Distance(transform.position, PlayerController.Instance.startPos);
      float duration = distance / speed;

      Vector3 movePoint = new Vector3(PlayerController.Instance.startPos.x, -6.59f);

      transform.DOMove(movePoint, duration).SetEase(Ease.Linear).OnComplete(() =>
      {
        PlayerController.Instance.activeBallCount++;

        if(PlayerController.Instance.activeBallCount == PlayerController.Instance.ps.additionalBallCount + 1)
        {
          PlayerController.Instance.isStartPosFixed = false;
          PlayerController.Instance.IsReady();
        }

        if(!isFirstDown)
        {
          Destroy(gameObject);
        }
      });
    }
  }
}
