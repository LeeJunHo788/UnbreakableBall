using UnityEngine;

[CreateAssetMenu(menuName = "Augments/Effects/Upgrade/Split_UpgradeAtt")]
public class Split_UpgradeAtt : ScriptableObject, IAugmentEffect
{
  public void Apply(AugmentRuntimeContext ctx)
  {
    ctx.manager.splitAM.attReduceVal += ctx.manager.splitAM.attReduceVal;
  }
}
