using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
  public SpriteRenderer blackPanel;
  public GameObject gameOverUI;
  public static GameManager Instance { get; set; }

  private void Awake()
  {
    if (Instance != null && Instance != this)
    {
      Destroy(gameObject);
      return;
    }
    Instance = this;
    DontDestroyOnLoad(gameObject);
  }


  public void ForcedReady()
  {
    if (!PlayerController.Instance.canReady) return;

    if(PlayerController.Instance.isStartPosFixed)
    {
      Debug.Log("위치 고정");

      PlayerController.Instance.rb.linearVelocity = Vector2.zero;
      PlayerController.Instance.transform.position = new Vector3(PlayerController.Instance.startPos.x, -6.59f);
      PlayerController.Instance.transform.rotation = Quaternion.Euler(0, 0, 90);
      PlayerController.Instance.isStartPosFixed = false;
      PlayerController.Instance.IsReady();
    }

    else
    {
      Debug.Log("위치 고정 안됨");
      PlayerController.Instance.rb.linearVelocity = Vector2.zero;

      PlayerController.Instance.transform.position = new Vector3(0, -6.59f);
      PlayerController.Instance.startPos = new Vector3(PlayerController.Instance.transform.position.x, -6.57f);
      PlayerController.Instance.isStartPosFixed = true;  // 고정 완료

      PlayerController.Instance.transform.rotation = Quaternion.Euler(0, 0, 90);
      PlayerController.Instance.activeBallCount++;

      PlayerController.Instance.isStartPosFixed = false;
      PlayerController.Instance.IsReady();
    }

  }

  public void GameOver()
  {
    blackPanel.DOFade(240, 1.5f).OnComplete(() =>
    {
      PlayerController.Instance.isGameOver = true;
      gameOverUI.SetActive(true);
    });

  }

  public void RestartGame()
  {
    blackPanel.DOFade(0, 1f).OnComplete(() =>
    {
      PlayerController.Instance.isGameOver = false;
      ExpManager.Instance.ResetExp();
      gameOverUI.SetActive(false);
    });
    

  }
}
