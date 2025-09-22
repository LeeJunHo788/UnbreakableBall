using UnityEngine;
using DG.Tweening;

public class CanonBallController : MonoBehaviour
{
  private PlayerController pc;

  CanonAugmentManager cam;

  public void Init(CanonAugmentManager canonAugmentManager)
  {
    cam = canonAugmentManager;
    pc = PlayerController.Instance;
  }

  private void Explode()
  {
    Collider2D[] inRangeBlocks = Physics2D.OverlapCircleAll(transform.position, cam.radius);
    foreach (var block in inRangeBlocks)
    {
      var blockComp = block.GetComponent<Block>();
      if(blockComp)
        blockComp.TakeDamage(pc.ps.att * cam.canonAtt);
    }

    float effectRadius = cam.radius / 5;

    var effect = Instantiate(cam.explodeParticle, transform.position, Quaternion.identity);
    var particle = effect.GetComponent<ParticleSystem>();
    var main = particle.main;
    main.startLifetime = effectRadius;
    particle.Play(true);

    Destroy(gameObject);
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.gameObject.CompareTag("SideBar") || collision.gameObject.CompareTag("Block"))
    {
      Explode();
    }
  }

}
