using UnityEngine;

[CreateAssetMenu(menuName = "Augments/Effects/UpgradeCanonAtt")]
public class UpgradeCanonAtt : ScriptableObject, IAugmentEffect
{
  public float amount = 10f;

  public void Apply(AugmentRuntimeContext ctx)
  {
    ctx.manager.canonAM.canonAtt += amount;
  }
}
