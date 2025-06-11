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
    // 공과 닿았을 때 체력 감소
    if (collision.gameObject.CompareTag("Player"))
    {
      Debug.Log("실행");

      PlayerController pc = collision.gameObject.GetComponent<PlayerController>();

      float dem = Mathf.RoundToInt(GetRandomAround(pc.att));

      hp -= dem;
      hpText.text = hp.ToString();

      if (hp <= 0)
      {
        Destroy(gameObject);
      }

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

}
