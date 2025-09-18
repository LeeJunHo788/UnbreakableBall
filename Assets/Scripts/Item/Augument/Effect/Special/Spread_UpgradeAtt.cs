using UnityEngine;

[CreateAssetMenu(menuName = "Augments/Effects/Upgrade/Spread_UpgradeAtt")]
public class Spread_UpgradeAtt : ScriptableObject, IAugmentEffect
{
  public float val = 0.2f;

  public void Apply(AugmentRuntimeContext ctx)
  {
    ctx.manager.spreadAM.spreadAtt = val;
  }
}
