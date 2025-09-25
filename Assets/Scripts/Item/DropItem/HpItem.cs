using UnityEngine;

public class HpItem : Item
{
		protected override void Apply(PlayerController pc)
		{
				pc.ps.hp += 3;
				pc.ps.HpSet();
		}
}
