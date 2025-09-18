using UnityEngine;

[CreateAssetMenu(menuName = "Augments/Reactive/Trajectory Arrow")]
public class TrajectoryArrow : ScriptableObject, IAugmentReactive
{
		private PlayerController pc;
		private TrajectoryArrowManager manager;

		public void Bind(AugmentManager mgr, in AugmentRuntimeContext ctx)
		{
				pc = ctx.Player.GetComponent<PlayerController>();
				manager = mgr.GetComponent<TrajectoryArrowManager>();
				if (manager == null)
						manager = mgr.gameObject.AddComponent<TrajectoryArrowManager>();

				// 이벤트 구독
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

		private void HandleFire() => manager?.Hide();
		private void HandleReady() => manager?.Show();
}