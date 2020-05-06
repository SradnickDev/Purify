using Animateables;
using Units.Player.General;
using UnityEngine;
using UnityEngine.UI;
using Utilities;
using Zenject;

#pragma warning disable 0649
namespace Battle
{
	[RequireComponent(typeof(Button))]
	[RequireComponent(typeof(AnimateableScale))]
	public class EndTurnButton : MonoBehaviour
	{
		[SerializeField] private AnimateableScale m_animateableScale;
		[Inject]
		private Player m_player;

		private Button m_button;
		private int m_previousEnergy = 0;
		private bool m_isActive;

		private void Start()
		{
			m_button = GetComponent<Button>();
			m_button.onClick.AddListener(() =>
			{
				m_button.OnDeselect(null);
			});
		}

		private void Update()
		{
			ValidateTurnEnd();
		}

		private void ValidateTurnEnd()
		{
			if (m_player == null || m_player.IsDead()) return;

			var hasEnergy = m_previousEnergy > 0;
			if (m_player.Energy.Current != m_previousEnergy)
			{
				hasEnergy = m_player.Energy.Current > 0;
				m_previousEnergy = m_player.Energy.Current;
				m_isActive = false;
			}

			var canPlayCards = false;

			foreach (var card in m_player.Hand.Cards)
			{
				if (card.CanPlay(m_player))
				{
					canPlayCards = true;
				}
			}

			if (!canPlayCards && !hasEnergy && !m_isActive)
			{
				m_animateableScale.Play();
				m_button.Select();
				m_isActive = true;
			}
		}
	}
}
#pragma warning restore 0649