using Battle.General;
using Deck;
using Misc;
using Misc.PopUp;
using Status;
using Units.Enemy.General;
using Units.Player.General;
using UnityEngine;
using Utilities;
using Zenject;

#pragma warning disable 0649
namespace Cards.General
{
	public class Hand : CardPile
	{
		[Inject]
		private DiscardPile m_discardPile = null;

		[Inject]
		private BanishPile m_banishPile = null;

		[Inject]
		private BlessingPile m_blessingPile;

		[Inject]
		private Player m_player;

		public CardInstance LastPlayedCard { get; private set; }

		public DiscardPileType DiscardPileType = DiscardPileType.Default;

		public bool CanPlayCard(CardInstance card) => card.CanPlay(m_player);

		public void Play(CardInstance card, Enemy enemy)
		{
			if (card.CanPlay(m_player))
			{
				LastPlayedCard = card;
				card.Play(enemy);
				BattleInfo.Player.Energy.Current -= card.CardData.Energy;
				CardTurnLog.Add(card.CardData.Type);

				if (card.CardData.Type == CardType.Blessing)
				{
					DiscardPileType = DiscardPileType.None;
				}

				Discard(card);
			}
		}

		public void Discard(CardInstance card)
		{
			if (card.CanDiscard(m_player))
			{
				PopUpHandler.Instance.CloseAll();
				Remove(card);

				switch (DiscardPileType)
				{
					case DiscardPileType.Default:
						m_discardPile.Add(card);
						break;
					case DiscardPileType.BanishPile:
						m_banishPile.Add(card);
						break;
					case DiscardPileType.None:
						m_blessingPile.Add(card);
						break;
				}

				DiscardPileType = DiscardPileType.Default;
			}
		}

		public void DiscardAll()
		{
			for (var i = Cards.Count - 1; i >= 0; i--)
			{
				var card = Cards[i];
				if (card.CanDiscard(m_player))
				{
					PopUpHandler.Instance.CloseAll();
					Remove(card);
					m_discardPile.Add(card);
				}
			}
		}

		public CardInstance GetRandomCard()
		{
			if (Count <= 1) return null;

			var retVal = Cards.RandomOne();

			if (retVal == LastPlayedCard)
			{
				return GetRandomCard();
			}

			return retVal;
		}
	}
}
#pragma warning restore 0649