using UnityEngine;

public class BallAddItem : MonoBehaviour
{
  private void OnTriggerEnter2D(Collider2D collision)
  {
    if(collision.CompareTag("Player"))
    {
      PlayerController.Instance.addBall = true;
      Destroy(gameObject);
    }
  }

}
