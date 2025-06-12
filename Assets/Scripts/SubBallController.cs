using UnityEngine;
using DG.Tweening;

public class SubBallController : MonoBehaviour
{
  public float att;
  public float defIg;

  private void Start()
  {
    att = PlayerController.Instance.att;
    defIg = PlayerController.Instance.defIg;
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.collider.CompareTag("DSideBar"))
    {
      float speed = 20;

      if (!PlayerController.Instance.isStartPosFixed)
      {
        PlayerController.Instance.startPos = transform.position;
        PlayerController.Instance.isStartPosFixed = true;
      }


      float distance = Vector3.Distance(transform.position, PlayerController.Instance.startPos);
      float duration = distance / speed;

      transform.DOMove(PlayerController.Instance.startPos, duration).SetEase(Ease.Linear).OnComplete(() =>
      {
        PlayerController.Instance.activeBallCount++;

        if(PlayerController.Instance.activeBallCount == PlayerController.Instance.additionalBallCount + 1)
        {
          PlayerController.Instance.activeBallCount = 0;
          PlayerController.Instance.isStartPosFixed = false;
          PlayerController.Instance.IsReady();
        }

        // if()
        Destroy(gameObject);
      });
    }
  }
}
