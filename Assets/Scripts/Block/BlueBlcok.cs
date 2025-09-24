using UnityEngine;

public class BlueBlcok : Block
{
  protected override void Start()
  {
    hp = StageManager.Instance.stageNum;
    maxHp = hp;
    def = 0;

    base.Start();
  }

}
