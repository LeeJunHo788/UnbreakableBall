using UnityEngine;

public class BlueBlcok : Block
{
  protected override void Start()
  {
    hp = 20f;
    def = 0;

    base.Start();
  }

  protected override void DropItem()
  {
    Debug.Log("½ÇÇà");
  }

}
