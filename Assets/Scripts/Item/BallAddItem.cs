using UnityEngine;

public class BallAddItem : MonoBehaviour
{
  private void OnTriggerEnter2D(Collider2D collision)
  {
    if(collision.CompareTag("Player"))
    {
      PlayerController.Instance.ps.AddBallCount();
      Destroy(gameObject);
    }
  }

}
