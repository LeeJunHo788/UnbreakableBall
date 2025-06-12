using UnityEngine;
using TMPro;
using DG.Tweening;

// �� ���� ���� = 1.2 ���� ���� = 0.6
public class Block : MonoBehaviour
{
  TextMeshPro hpText;

  protected float hp;
  protected float def;

  protected virtual void Start()
  {
    hpText = GetComponentInChildren<TextMeshPro>();
    hpText.text = hp.ToString();

    PlayerController.Instance.OnPlayerReady += MoveDown;

  }

  void OnCollisionEnter2D(Collision2D collision)
  {
    float dem = 0;

    // ���� ����� �� ü�� ����
    if (collision.gameObject.CompareTag("Player"))
    {
      PlayerController pc = collision.gameObject.GetComponent<PlayerController>();

      dem = Mathf.RoundToInt(GetRandomAround(pc.att));
    }

    else if (collision.gameObject.CompareTag("SubBall"))
    {
      SubBallController sc = collision.gameObject.GetComponent<SubBallController>();

      dem = Mathf.RoundToInt(GetRandomAround(sc.att));
    }

    float finalDem = Mathf.Max(dem - def, 1); // �ּ� ������ ����

    hp -= finalDem;
    hpText.text = hp.ToString();

    if (hp <= 0)
    {
      DropItem();
      Destroy(gameObject);
    }

  }

  // ������ ���� �̱�
  float GetRandomAround(float val)
  {
    return Random.Range(val - (val*0.25f), val + (val * 0.25f));
  }

  // �Ʒ� �̵� �Լ�
  void MoveDown()
  {
    transform.DOMoveY(transform.position.y - 0.6f, 0.35f);
  }

  void OnDestroy()
  {
    PlayerController.Instance.OnPlayerReady -= MoveDown;
  }

  protected virtual void DropItem() { }

}
