using System.Collections.Generic;
using System.Linq;
using Cards.General;
using Deck;
using Units.Player.General;
using UnityEngine;
using UnityEngine.Events;
using Utilities;
using Zenject;

#pragma warning disable 0649
namespace Battle.RewardMenu
{
	[RequireComponent(typeof(CardCollectionViewOpener))]
	public class RewardCardMenu : MonoBehaviour
	{
		public UnityEvent OnCardSelected;

		[Inject]
		private Player m_player;

		[SerializeField] private CardCollectionViewOpener CardCollectionViewOpener;
		[SerializeField] private int m_rewardCount = 3;

		private void Start()
		{
			CreateSelection();
		}

		private void CreateSelection()
		{
			var cards = LoadSelection();

			CardCollectionViewOpener.Open(cards, CardSelected, false);
		}

		private void CardSelected(CardModel cv)
		{
			AddReward(cv.Instance);
			CardCollectionViewOpener.Close();
		}

		/// <summary>
		/// Collection of unique reward cards.
		/// </summary>
		/// <returns></returns>
		private IEnumerable<CardInstance> LoadSelection()
		{
			var deck = DeckUtility.LoadChosenPool();

			var cardsData = DeckUtility.RarityBasedCards(deck, m_rewardCount);

			var retVal = cardsData.Select
			(
				data => new CardInstance(data)
			).ToList();

			return retVal;
		}

		/// <summary>
		/// Add reward card to Player deck and save data.
		/// </summary>
		/// <param name="card"></param>
		public void AddReward(CardInstance card)
		{
			m_player.CardDeck.Add(card);
			DeckUtility.SaveDeck(m_player.CardDeck);

			OnCardSelected?.Invoke();
		}
	}
}
#pragma warning restore 0649