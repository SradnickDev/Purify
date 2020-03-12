using Deck;
using Units.Player.General;
using UnityEngine;
using Zenject;

#pragma warning disable 0649
namespace OptionMenu
{
	public class InGameSettings : MonoBehaviour
	{
		[Inject] private Player m_player;
		[SerializeField] private CardCollectionViewOpener m_viewOpener;
		[SerializeField] private GameObject m_options;
		[SerializeField] private GameObject m_background;

		private bool m_deckIsOpen = false;
		private bool m_optionsIsOpen = false;

		public void OpenOrCloseOptions()
		{
			m_optionsIsOpen = !m_optionsIsOpen;
			m_options.SetActive(m_optionsIsOpen);
			if (m_optionsIsOpen)
			{
				m_viewOpener.Close();
				m_deckIsOpen = false;
			}

			HandleBackground();
		}

		public void OpenOrCloseDeckView()
		{
			m_deckIsOpen = !m_deckIsOpen;
			if (m_deckIsOpen)
			{
				m_viewOpener.Open(m_player.CardDeck, null, true);
				m_options.SetActive(false);
				m_optionsIsOpen = false;
			}
			else
			{
				m_viewOpener.Close();
			}

			HandleBackground();
		}

		private void HandleBackground()
		{
			if (m_deckIsOpen || m_optionsIsOpen)
			{
				m_background.SetActive(true);
			}

			if (!m_deckIsOpen && !m_optionsIsOpen)
			{
				m_background.SetActive(false);
			}
		}
	}
}
#pragma warning restore 0649