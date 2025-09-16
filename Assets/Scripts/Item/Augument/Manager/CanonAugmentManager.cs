using UnityEngine;
using DG.Tweening;

public class CanonAugmentManager : MonoBehaviour
{
  private bool isActive = false;
  private PlayerController pc;

  private Vector2 spawnPos;
  private float canonSpeed = 15;
  private float duration = 0.5f;

  [Header("할당 오브젝트")]
  public GameObject canon;
  public GameObject canonBall;
  public GameObject explodeParticle;

  public float canonAtt = 10;
  public int canonBallCount = 1;
  public float radius = 1;

 
  public void Activate(AugmentRuntimeContext ctx)
  {
    pc = ctx.Player.GetComponent<PlayerController>();

    if(!isActive)
    {
      isActive = true;
      pc.OnPlayerFire += SpawnCanonFire;
    }
  }

  public void Deactivate()
  {
    if (isActive)
    {
      isActive = false;
      pc.OnPlayerFire -= SpawnCanonFire;
    }
  }

  private void SpawnCanonFire()
  {
    spawnPos = pc.startPos;
    GameObject tempcanon = Instantiate(canon, spawnPos, Quaternion.identity);

    tempcanon.transform.localScale = Vector3.zero;

    Sequence seq = DOTween.Sequence();

    seq.Append(tempcanon.transform.DOScale(1, duration));

    seq.AppendCallback(() =>
    {
      Fire();
    });

    seq.AppendInterval(1f);
    seq.AppendCallback(() =>
    {
      DestroyCanon(tempcanon);
    });
  }
    

  private void Fire()
  {
    Sequence seq = DOTween.Sequence();
    for (int i = 0; i < canonBallCount; i++)
    {
      int index = i;

      seq.AppendCallback(() =>
      {
        var tempCanonBall = Instantiate(canonBall, spawnPos, Quaternion.identity);
        var cc = tempCanonBall.GetComponent<CanonBallController>();
        cc.Init(this);

        var rb = tempCanonBall.GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.up * canonSpeed;
      });

      seq.AppendInterval(1f);

    }
  }

  private void DestroyCanon(GameObject tempCanon)
  {
    tempCanon.transform.DOScale(0, duration).OnComplete(() =>
    {
      Destroy(tempCanon);
    });
  }
  
}
