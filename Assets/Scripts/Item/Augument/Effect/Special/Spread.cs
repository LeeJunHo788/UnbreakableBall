using UnityEngine;

[CreateAssetMenu(menuName = "Augments/Reactive/Spread")]
public class Spread : ScriptableObject, IAugmentReactive
{
		public void Bind(AugmentManager manager, in AugmentRuntimeContext ctx)
		{
				manager.spreadAM.Activate(ctx);
		}

		public void Unbind(AugmentManager manager)
		{
				manager.spreadAM.Deactivate();
		}



}
