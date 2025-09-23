using UnityEngine;

[CreateAssetMenu(menuName = "Augments/Effects/Upgrade/Trajectory_UpgradeReflectionCount")]
public class Trajectory_UpgradeReflectionCount : ScriptableObject, IAugmentEffect
{
  public void Apply(AugmentRuntimeContext ctx)
  {
    ctx.manager.trajectoryAm.bounceCount++;
  }
}
