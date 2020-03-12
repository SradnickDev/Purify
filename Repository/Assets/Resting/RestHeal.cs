using Units.Player.General;
using UnityEngine;
using Zenject;

namespace Resting
{
	public class RestHeal : RestMechanic
	{
		[SerializeField] private int HealthAmount = 0;

		[Inject]
		private Player m_player = null;

		protected override void UpdateAvailableStatus()
		{
			if (RestMenu)
			{
				Button.interactable = RestMenu.HasRestPoints(Costs) && m_player.Health.Current < m_player.Health.Max;
			}
		}

		public override void ApplyMechanic()
		{
			if (m_player.Health.Current >= m_player.Health.Max) return;
			if (m_player)
			{
				m_player.Health.Current += HealthAmount;
			}

			if (RestMenu.BattleConfig.Health != null)
			{
				RestMenu.BattleConfig.Health.Min = m_player.Health.Current;
			}

			RestMenu.UseRestPoints(Costs);
			RestMenu.Save();
		}
	}
}