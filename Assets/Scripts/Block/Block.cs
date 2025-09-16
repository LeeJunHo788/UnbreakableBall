using UnityEngine;
using TMPro;
using DG.Tweening;

// 블럭 가로 차이 = 1 세로 차이 = 0.5
public class Block : MonoBehaviour
{
  TextMeshPro hpText;

  protected float maxHp;
  protected float hp;
  protected float def;
  protected int advanceCount = 0;

  protected virtual void Start()
  {
    hpText = GetComponentInChildren<TextMeshPro>();
    hpText.text = Mathf.RoundToInt(hp).ToString();

    PlayerController.Instance.OnPlayerReady += MoveDown;

  }

  void OnCollisionEnter2D(Collision2D collision)
  {
    float dem = 0;

    // 공과 닿았을 때 체력 감소
    if (collision.gameObject.CompareTag("Player"))
    {
      PlayerController pc = collision.gameObject.GetComponent<PlayerController>();

      dem = Mathf.RoundToInt(GetRandomAround(pc.ps.att));
    }

    else if (collision.gameObject.CompareTag("SubBall"))
    {
      SubBallController sc = collision.gameObject.GetComponent<SubBallController>();

      dem = Mathf.RoundToInt(GetRandomAround(sc.ss.att));
    }

    TakeDamage(dem);

  }

  // 데미지 랜덤 뽑기
  float GetRandomAround(float val)
  {
    return Random.Range(val - (val*0.25f), val + (val * 0.25f));
  }

  // 아래 이동 함수
  void MoveDown()
  {
    if(advanceCount == 25)
    {
      SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();
      TMP_Text[] tmps = GetComponentsInChildren<TMP_Text>(true);

      PlayerController.Instance.ps.TakeDamage(Mathf.RoundToInt(hp));

      Sequence seq = DOTween.Sequence();

      foreach (var rend in renderers)
      {
        seq.Join(rend.DOFade(0f, 1f));
      }

      foreach (var t in tmps)
      {
        seq.Join(t.DOFade(0f, 1f));
      }

      seq.OnComplete(() =>
      {
        Destroy(gameObject); 
      });

    }

    transform.DOMoveY(transform.position.y - 0.5f, 0.35f);
    advanceCount++;
  }

  void OnDestroy()
  {
    PlayerController.Instance.OnPlayerReady -= MoveDown;
  }

  public void TakeDamage(float dem)
  {
    hp -= dem;
    // float finalDem = Mathf.Max(dem - def, 1); // 최소 데미지 보정
    // hp -= finalDem;

    hpText.text = Mathf.RoundToInt(hp).ToString();

    if (hp <= 0)
    {
      ExpManager.Instance.AddExp(maxHp);
      DropItem();
      Destroy(gameObject);
    }
  }

  protected virtual void DropItem() { }

}
