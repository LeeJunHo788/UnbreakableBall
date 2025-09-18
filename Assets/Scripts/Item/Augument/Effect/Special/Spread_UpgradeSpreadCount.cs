using UnityEngine;

[CreateAssetMenu(menuName = "Augments/Effects/Upgrade/Spread_UpgradeSpreadCount")]
public class Spread_UpgradeSpreadCount : ScriptableObject, IAugmentEffect
{
  public void Apply(AugmentRuntimeContext ctx)
  {
    ctx.manager.spreadAM.spreadNum += 2;
  }
}
