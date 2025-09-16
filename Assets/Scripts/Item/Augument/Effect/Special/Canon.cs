using UnityEngine;

[CreateAssetMenu(menuName = "Augments/Reactive/Canon")]
public class Canon : ScriptableObject, IAugmentReactive
{
  public void Bind(AugmentManager manager, in AugmentRuntimeContext ctx)
  {
    manager.canonAM.Activate(ctx);
  }

  public void Unbind(AugmentManager manager)
  {
    manager.canonAM.Deactivate();
  }


    
}
