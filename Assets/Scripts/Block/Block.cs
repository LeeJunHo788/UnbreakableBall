using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections.Generic;

// 블럭 가로 차이 = 1 세로 차이 = 0.5
public class Block : MonoBehaviour
{
  TextMeshPro hpText;

  protected float maxHp;
  protected float hp;
  protected float def;
  protected int advanceCount = 0;

		[HideInInspector] public int spawnWeight;

		public virtual int GetWeight(int round)
		{
				return spawnWeight;
		}

		[Header("드랍 아이템 리스트")]
		public List<Item> dropItemList;

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

    hpText.text = Mathf.RoundToInt(hp).ToString();

    if (hp <= 0)
    {
      ExpManager.Instance.AddExp(maxHp);

						GameObject dropItem = TryDropItem();

						if (dropItem != null)
								Instantiate(dropItem, transform.position, Quaternion.identity);

      Destroy(gameObject);
    }
  }

  protected GameObject TryDropItem()
		{
				if (dropItemList.Count == 0) return null;

				float rnd = Random.Range(0f, 1f);
				if (rnd > PlayerController.Instance.ps.dropChance) return null;

				float totalWeight = 0;
				foreach (var item in dropItemList)
				{
						totalWeight += item.dropWeight;
				}

				float rndValue = Random.Range(0, totalWeight);

				int acc = 0;
				foreach (var item in dropItemList)
				{
						acc += item.dropWeight;
						if (rndValue < acc)
						{
								return item.gameObject;
						}
				}
				return null;
				
		}

		
}
		