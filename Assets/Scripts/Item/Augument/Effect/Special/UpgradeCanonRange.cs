using UnityEngine;

[CreateAssetMenu(menuName = "Augments/Effects/UpgradeCanonRange")]
public class UpgradeCanonRange : ScriptableObject, IAugmentEffect
{
  public float amount = 0.5f;

  public void Apply(AugmentRuntimeContext ctx)
  {
    ctx.manager.canonAM.radius += amount;
  }
}
