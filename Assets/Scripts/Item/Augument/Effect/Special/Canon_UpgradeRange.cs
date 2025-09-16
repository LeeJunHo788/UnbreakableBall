using UnityEngine;

[CreateAssetMenu(menuName = "Augments/Effects/Upgrade/Canon_UpgradeRange")]
public class Canon_UpgradeRange : ScriptableObject, IAugmentEffect
{
  public float amount = 0.5f;

  public void Apply(AugmentRuntimeContext ctx)
  {
    ctx.manager.canonAM.radius += amount;
  }
}
