using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
  public Image blackPanel;
  public GameObject gameOverUI;
  public BlockSpawner bs;
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
				Debug.Log("호출");

    if (!PlayerController.Instance.canReady || PlayerController.Instance.isGameOver ) return;

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
    PlayerController.Instance.isGameOver = true;

    blackPanel.DOFade(0.8f, 1.5f).OnComplete(() =>
    {
      gameOverUI.SetActive(true);
    });

  }

  public void RestartGame()
  {
    gameOverUI.SetActive(false);
    blackPanel.DOFade(0, 1f).OnComplete(() =>
    {
      PlayerController.Instance.ps.StartGame();
      ExpManager.Instance.ResetExp();
      StageManager.Instance.stageNum = 1;
      bs.RestartGame();

      PlayerController.Instance.isGameOver = false;
    });
    

  }
}
