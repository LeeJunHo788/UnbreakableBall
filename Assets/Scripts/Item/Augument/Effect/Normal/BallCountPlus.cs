using UnityEngine;

[CreateAssetMenu(menuName = "Augments/Effects/BallCountPlus")]
public class BallCountPlus : ScriptableObject, IAugmentEffect
{
  public int amount = 1;

  public void Apply(AugmentRuntimeContext ctx)
  {
    if (ctx.Stats != null)
    {
      ctx.Stats.additionalBallCount += amount;
    }
  }

}