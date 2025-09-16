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
    var pc = PlayerController.Instance;
    if (!pc.canForceReady || pc.isGameOver ) return;

    if(pc.isStartPosFixed)
    {

      pc.rb.linearVelocity = Vector2.zero;
      pc.transform.position = new Vector3(pc.startPos.x, -6.59f);
      pc.transform.rotation = Quaternion.Euler(0, 0, 90);
      pc.isStartPosFixed = false;
      pc.activeBallCount = pc.ReturnBallThisRound;
      pc.IsReady();
    }

    else
    {
      pc.rb.linearVelocity = Vector2.zero;

      pc.transform.position = new Vector3(0, -6.59f);
      pc.startPos = new Vector3(pc.transform.position.x, -6.57f);
      pc.isStartPosFixed = true;  // 고정 완료

      pc.transform.rotation = Quaternion.Euler(0, 0, 90);
      pc.activeBallCount++;

      pc.isStartPosFixed = false;
      pc.activeBallCount = pc.ReturnBallThisRound;
      pc.IsReady();
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
