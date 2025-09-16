using UnityEngine;

[CreateAssetMenu(menuName = "Augments/Effects/UpgradeSplitAtt")]
public class UpgradeSplitBallAtt : ScriptableObject, IAugmentEffect
{
  public void Apply(AugmentRuntimeContext ctx)
  {
    ctx.manager.splitAM.attReduceVal += ctx.manager.splitAM.attReduceVal;
  }
}
