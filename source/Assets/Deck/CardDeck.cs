using System.Collections.Generic;
using Cards.General;

namespace Deck
{
	[System.Serializable]
	public class CardDeck : CardPile
	{
		public IReadOnlyList<CardInstance> GetAll()
		{
			return Cards;
		}
	}
}