using Units.Player.General;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 0649
namespace Cards.General
{
	[RequireComponent(typeof(KeywordHandler))]
	public class CardModel : MonoBehaviour
	{
		[HideInInspector]public string Description;
		public Player Player => (Player) m_instance.Owner;
		[SerializeField] private KeywordHandler m_keywordHandler;

		[Header("Rarity")]
		[SerializeField] private Image m_rarityIndicator;
		[SerializeField] private Image m_ornaments;

		[SerializeField] private Color m_starter;
		[SerializeField] private Color m_uncommon;
		[SerializeField] private Color m_common;
		[SerializeField] private Color m_rar;

		public CardInstance Instance
		{
			get => m_instance;
		}

		private CardInstance m_instance;

		public void Setup(CardInstance instance)
		{
			SetColor(instance);
			SetDescription(instance);
			m_instance = instance;
		}

		private void SetDescription(CardInstance instance)
		{
			var parsedDescription = m_keywordHandler.ParseKeywords(instance.CardData.Description);
			Description = parsedDescription;
		}

		private void SetColor(CardInstance instance)
		{
			switch (instance.CardData.Rarity)
			{
				case Rarity.Starter:
					m_rarityIndicator.color = m_starter;
					m_ornaments.color = m_starter;
					break;
				case Rarity.Uncommon:
					m_rarityIndicator.color = m_uncommon;
					m_ornaments.color = m_uncommon;
					break;
				case Rarity.Common:
					m_rarityIndicator.color = m_common;
					m_ornaments.color = m_common;
					break;
				case Rarity.Rar:
					m_rarityIndicator.color = m_rar;
					m_ornaments.color = m_rar;
					break;
			}
		}
	}
}
#pragma warning restore 0649