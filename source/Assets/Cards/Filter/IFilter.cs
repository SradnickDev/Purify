using System;
using System.Collections.Generic;
using Cards.General;

namespace Cards.Filter
{
	public interface IFilter<T>
	{
		IEnumerable<T> Collection(IEnumerable<T> cards, Func<CardData, bool> specification);
		T Single(IEnumerable<T> cards, Func<CardData, bool> specification);
	}
}