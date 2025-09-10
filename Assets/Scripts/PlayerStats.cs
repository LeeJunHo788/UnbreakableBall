using UnityEngine;
using TMPro;

public class PlayerStats : MonoBehaviour
{
  public TextMeshProUGUI hpText;


  [HideInInspector] public float att = 1;
  [HideInInspector] public float defIg = 0;
  [HideInInspector] public int additionalBallCount = 0;
  [HideInInspector] public float moveSpeed = 10;
  [HideInInspector] public float shootInterval = 0.075f;
  [HideInInspector] public int hp = 10;


  [HideInInspector] public int isAdded = 0;

  void Start()
  {
    StartGame();
  }

  public void TakeDamage(int val)
  {
    hp -= val;
    hpText.text = $"Hp : {hp}";

    if(hp <= 0)
    {
      GameManager.Instance.GameOver();
    }
  }

  public void AddBallCount()
  {
				isAdded++;
  }

  public void StartGame()
  {
    att = 1;
    defIg = 0;
    additionalBallCount = 0;
    moveSpeed = 10;
    shootInterval = 0.075f;
    hp = 10;
    hpText.text = $"Hp : {hp}";
  }
}
