using UnityEngine;

[CreateAssetMenu(menuName = "Augments/Reactive/Trajectory Arrow")]
public class TrajectoryArrow : ScriptableObject, IAugmentReactive
{

		public void Bind(AugmentManager mgr, in AugmentRuntimeContext ctx)
		{
				mgr.trajectoryAm.Activate(ctx);
  }

		public void Unbind(AugmentManager mgr)
		{
				mgr.trajectoryAm.Deactivate();
		}
}