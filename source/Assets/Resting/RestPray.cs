using Units.Player.General;
using UnityEngine;
using Zenject;

namespace Resting {
	public class RestPray : RestMechanic
	{
		[SerializeField] private int SoulAmount = 0;

		[Inject]
		private Player m_player = null;

		public override void ApplyMechanic()
		{
			RestMenu.BattleConfig.Soul += SoulAmount;
			if (m_player)
			{
				m_player.Soul.Current += SoulAmount;
			}

			RestMenu.UseRestPoints(Costs);
		
			RestMenu.Save();
		}
	}
}