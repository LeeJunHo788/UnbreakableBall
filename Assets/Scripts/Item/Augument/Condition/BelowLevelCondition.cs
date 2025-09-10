using UnityEngine;

[CreateAssetMenu(menuName = "Augments/Conditions/BelowLevelCondition")]
public class BelowLevelCondition : ScriptableObject, IAugmentCondition
{
  public int requiredLevel = 1;

  public bool Evaluate(in AugmentCheckContext ctx)
  {
    return ctx.CurrentLevel <= requiredLevel;
  }
    
}
