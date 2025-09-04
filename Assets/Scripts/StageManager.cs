using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class StageManager : MonoBehaviour
{

  public static StageManager Instance { get; set; }

  void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
      DontDestroyOnLoad(gameObject);
    }

    else Destroy(gameObject);
  }

  void Start()
  {
    // textStageNum.text = $"Stage : {stageNum.ToString()}";

  }

  public int stageNum = 1;

  public void NextStage()
  {
    stageNum++;
    // textStageNum.text = $"Stage : {stageNum.ToString()}";
  }
}
