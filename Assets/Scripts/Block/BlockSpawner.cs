using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class BlockSpawner : MonoBehaviour
{
		public AugmentManager augmentManager;

		[Header("��� ������")]
		public List<Block> blocks = new List<Block>();
  Transform[] spawnPoints;

  private void Start()
  {
    // ��ȯ ��ġ ��������
    spawnPoints = new Transform[transform.childCount];
    for (int i = 0; i < transform.childCount; i++)
    {
      spawnPoints[i] = transform.GetChild(i);
    }


    PlayerController.Instance.OnPlayerReady += SpawnBlock;  // �̺�Ʈ ����
				augmentManager.OnAugmentFinished += SpawnBlock;
  }

  // ������ ��ġ�� �� ��ȯ
  public void SpawnBlock()
  {
				if (ExpManager.Instance._pendingAugmentCount != 0) return;

    int count = Random.Range(1, spawnPoints.Length);

    List<int> rnd = new List<int>();

    while (rnd.Count < count)
    {
      int num = Random.Range(0, spawnPoints.Length);
      if(!rnd.Contains(num))
      {
        rnd.Add(num);
      }
    }

    foreach (int n in rnd)
    {
						GameObject temp = ChoiceBlock();

						if(temp != null)
						{
								GameObject block = Instantiate(temp, spawnPoints[n].transform.position, Quaternion.identity);
								block.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
								block.transform.DOScale(new Vector3(1.0f, 0.5f, 1), 0.3f).SetEase(Ease.InOutSine);

						}


    }

  }

		public GameObject ChoiceBlock()
		{
				int total = 0;

				foreach(var b in blocks)
				{
						total += b.spawnWeight;
				}

				int rnd = Random.Range(0, total);

				int acc = 0;
				foreach (var b in blocks)
				{
						acc += b.spawnWeight;
						if (rnd < acc)
						{
								return b.gameObject;
						}
				}
				return null;

		}

  void OnDestroy()
  {
    PlayerController.Instance.OnPlayerReady -= SpawnBlock;  // �̺�Ʈ ���� ����
				augmentManager.OnAugmentFinished -= SpawnBlock;
  }

  public void RestartGame()
  {
    Block[] blocks = FindObjectsByType<Block>(FindObjectsSortMode.None);

    foreach (Block block in blocks)
    {
      Destroy(block.gameObject);
    }

    SpawnBlock();

  }

}
