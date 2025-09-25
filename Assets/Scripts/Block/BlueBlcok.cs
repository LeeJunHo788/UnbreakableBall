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

		public override int GetWeight(int round)
		{
				if (round <= 10)
						return spawnWeight = 10;

				else if (round <= 20)
						return spawnWeight = 8;

				else if (round <= 30)
						return spawnWeight = 5;
		}

}
