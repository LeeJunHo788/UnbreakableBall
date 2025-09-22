using UnityEngine;

public class SpreadObjectController : MonoBehaviour
{
  private float att;
  
  public void Init(float val)
  {
    att = val;
  }

  void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.gameObject.CompareTag("Block"))
    {
      Block bc = collision.gameObject.GetComponent<Block>();
      bc.TakeDamage(att);
      Destroy(gameObject);
    }

    else if (!(collision.gameObject.layer == LayerMask.NameToLayer("Ball")))
    {
      Destroy(gameObject);
    }
  }
}
