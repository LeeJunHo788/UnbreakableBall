using UnityEngine;
using TMPro;
using DG.Tweening;

// �� ���� ���� = 1 ���� ���� = 0.5
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

    // ���� ����� �� ü�� ����
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

  // ������ ���� �̱�
  float GetRandomAround(float val)
  {
    return Random.Range(val - (val*0.25f), val + (val * 0.25f));
  }

  // �Ʒ� �̵� �Լ�
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
    // float finalDem = Mathf.Max(dem - def, 1); // �ּ� ������ ����
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
