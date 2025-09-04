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
  [HideInInspector] public int hp = 100;


  [HideInInspector] public bool isAdded = false;

  public void TakeDamage(int val)
  {
    hp -= val;
    hpText.text = $"Hp : {hp}";
  }

  public void AddBallCount()
  {
    isAdded = true;
  }

}
