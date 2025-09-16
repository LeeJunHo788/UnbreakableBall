using UnityEngine;

[CreateAssetMenu(menuName = "Augments/Effects/Upgrade/Canon_UpgradeAtt")]
public class Canon_UpgradeAtt : ScriptableObject, IAugmentEffect
{
  public float amount = 10f;

  public void Apply(AugmentRuntimeContext ctx)
  {
    ctx.manager.canonAM.canonAtt += amount;
  }
}
