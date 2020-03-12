using System;
using System.Collections.Generic;
using System.Linq;
using Cards.General;

namespace Cards.Filter
{
	public class CardDataFilter : IFilter<CardData>
	{
		public IEnumerable<CardData> Collection(IEnumerable<CardData> cards, Func<CardData, bool> specification)
		{
			return cards.Where(x => specification(x) == true);
		}

		public CardData Single(IEnumerable<CardData> cards, Func<CardData, bool> specification)
		{
			return cards.FirstOrDefault(x => specification(x) == true);
		}
	}

	public class CardInstanceFilter : IFilter<CardInstance>
	{
		public IEnumerable<CardInstance> Collection(IEnumerable<CardInstance> cards, Func<CardData, bool> specification)
		{
			return cards.Where(x => specification(x.CardData) == true);
		}

		public CardInstance Single(IEnumerable<CardInstance> cards, Func<CardData, bool> specification)
		{
			return cards.FirstOrDefault(x => specification(x.CardData) == true);
		}
	}
}