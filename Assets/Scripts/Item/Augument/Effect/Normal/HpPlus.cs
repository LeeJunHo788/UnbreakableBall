using UnityEngine;

[CreateAssetMenu(menuName = "Augments/Effects/HpPlus")]
public class HpPlus : ScriptableObject, IAugmentEffect
{
  public int amount = 1;

  public void Apply(AugmentRuntimeContext ctx)
  {
    if (ctx.Stats != null)
    {
      ctx.Stats.hp += amount;
    }
  }

}