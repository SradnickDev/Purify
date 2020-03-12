using System;
using System.Collections.Generic;
using Cards.Filter;
using Cards.General;
using Drawer;
using MessagePack;
using UnityEngine;

namespace Deck
{
	[MessagePackObject(true)]
	public class CardPool
	{
		public string Name;
		public int Id;
		public List<CardData> Cards = new List<CardData>();

		public void Add(CardData cardData)
		{
			Cards.Add(cardData);
			cardData.Id = Convert.ToInt32($"{Id}{Cards.Count}");
		}

		public void Remove(CardData cardData)
		{
			Cards.Remove(cardData);
		}

		public void Remove(int idx)
		{
			Cards.RemoveAt(idx);
		}

		/// <summary>
		/// Finds all matching objects based on specification.
		/// </summary>
		/// <param name="specification">specify the filter</param>
		/// <returns>Matching results</returns>
		public IEnumerable<CardData> GetCollection(Func<CardData, bool> specification)
		{
			var filter = new CardDataFilter();

			foreach (var card in filter.Collection(Cards, specification))
			{
				yield return card;
			}
		}

		/// <summary>
		/// Find first matching CardInstances based on specification.
		/// </summary>
		/// <param name="specification">specify the filter</param>
		/// <returns>Matching result</returns>
		public CardData GetSingle(Func<CardData, bool> specification)
		{
			var filter = new CardDataFilter();

			return filter.Single(Cards, specification);
		}
	}
}