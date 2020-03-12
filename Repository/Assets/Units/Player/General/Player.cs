using System.Collections;
using System.Collections.Generic;
using Cards.General;
using Deck;
using Stats.Types;
using Units.General;
using UnityEngine;
using Utilities;
using Zenject;
using ReadOnly = Drawer;

namespace Units.Player.General
{
	public class Player : Unit
	{
		public Energy Energy;

		[Header("Piles")]
		[Inject]
		public CardDeck CardDeck;

		[Inject]
		public Hand Hand;

		[Inject]
		public DrawPile DrawPile;

		[Inject]
		public DiscardPile DiscardPile;

		[Inject]
		public BanishPile BanishPile;

		public int HandSize { get; set; } = 5;

		public bool IsDone { get; private set; } = false;

		public void CreateHand()
		{
			StartCoroutine(DrawCards());
		}

		IEnumerator DrawCards()
		{
			IsDone = false;
			for (var i = 0; i < HandSize; i++)
			{
				if (DrawPile.Count > 0)
				{
					yield return new WaitForSeconds(0.1f);
					DrawCard();
				}
			}

			IsDone = true;
		}

		public void CreateDrawPile()
		{
			var cards = new List<CardInstance>(CardDeck.GetAll());
			cards.Shuffle();
			DrawPile.Cards = cards;
		}

		public void ReCreateDrawPile()
		{
			RetrieveCardsFromDiscard();
		}

		/// <summary>
		///  All Cards from discard pile goes into the draw pile.
		/// </summary>
		/// <returns></returns>
		private bool RetrieveCardsFromDiscard()
		{
			if (DiscardPile.Count == 0 || DrawPile.Count >= HandSize) return false;

			for (var i = DiscardPile.Cards.Count - 1; i >= 0; i--)
			{
				var card = DiscardPile.Cards[i];
				DiscardPile.Remove(card);
				DrawPile.Add(card);
			}

			return true;
		}

		/// <summary>
		/// Draw card from draw pile.
		/// If draw pile is empty retrieve cards (if available) from discard pile
		/// </summary>
		public void DrawCard()
		{
			if (!DrawPile.HasCard())
			{
				if (RetrieveCardsFromDiscard())
				{
					DrawCard();
				}

				return;
			}

			var newCard = DrawPile.Draw();
			Hand.Add(newCard);
		}

		private void OnDestroy()
		{
			CardDeck = null;
			Hand = null;
			DrawPile = null;
			DiscardPile = null;
			BanishPile = null;
		}
	}
}