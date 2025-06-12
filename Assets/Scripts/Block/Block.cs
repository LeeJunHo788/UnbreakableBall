using UnityEngine;
using TMPro;
using DG.Tweening;

// 블럭 가로 차이 = 1.2 세로 차이 = 0.6
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

    // 공과 닿았을 때 체력 감소
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

    float finalDem = Mathf.Max(dem - def, 1); // 최소 데미지 보정

    hp -= finalDem;
    hpText.text = hp.ToString();

    if (hp <= 0)
    {
      DropItem();
      Destroy(gameObject);
    }

  }

  // 데미지 랜덤 뽑기
  float GetRandomAround(float val)
  {
    return Random.Range(val - (val*0.25f), val + (val * 0.25f));
  }

  // 아래 이동 함수
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
