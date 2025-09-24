using UnityEngine;

public class BallAddItem : Item
{
		protected override void Apply(PlayerController pc)
		{
				pc.ps.additionalBallCount++;
		}
}
