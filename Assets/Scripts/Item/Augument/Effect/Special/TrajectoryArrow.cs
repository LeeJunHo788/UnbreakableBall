using UnityEngine;

[CreateAssetMenu(menuName = "Augments/Reactive/Trajectory Arrow")]
public class TrajectoryArrow : ScriptableObject, IAugmentReactive
{
		private PlayerController pc;
		private TrajectoryArrowManager manager;

		public void Bind(AugmentManager mgr, in AugmentRuntimeContext ctx)
		{
				pc = PlayerController.Instance;
				manager = mgr.trajectoryAm;

				// �̺�Ʈ ����
				pc.OnPlayerFire += HandleFire;
				pc.OnPlayerReady += HandleReady;

				if (pc.isReady) manager.Show();
				else manager.Hide();
		}

		public void Unbind(AugmentManager mgr)
		{
				if (pc != null)
				{
						pc.OnPlayerFire -= HandleFire;
						pc.OnPlayerReady -= HandleReady;
				}
				manager?.Hide();
				pc = null;
				manager = null;
		}

		private void HandleFire() => manager.Hide();
		private void HandleReady() => manager.Show();
}