using Battle.General;
using Units.Player.General;
using UnityEngine;
using UnityEngine.UI.Extensions;

#pragma warning disable 0649
namespace Cards.General
{
	public class CardPlayableEffect : MonoBehaviour
	{
		public bool IsActive { get; set; } = true;
		private Player m_player;
		[SerializeField] private CardModel m_card;
		[SerializeField] private UIParticleSystem m_particle;
		public bool ForceEnable { get; set; }

		private void Start()
		{
			m_player = BattleInfo.Player;
		}

		private void Update()
		{
			HandleGlow();
		}

		private void HandleGlow()
		{
			if (ForceEnable)
			{
				m_particle.enabled = true;
				return;
			}

			if (m_player && m_card)
			{
				m_particle.enabled = m_card.Instance.CanPlay(m_player);
			}
			else
			{
				m_particle.enabled = false;
			}
		}

		public void Reset()
		{
			m_player = null;
			m_card = null;
		}

		public void SetColor(Color color)
		{
			var mainModule = m_particle.GetComponent<ParticleSystem>().main;
			mainModule.startColor = color;
		}
	}
}
#pragma warning restore 0649