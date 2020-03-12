using System;
using System.Collections.Generic;
using Cards.Filter;
using Cards.General;
using UnityEngine;

namespace Deck
{
	public class CardPile
	{
#region Events

		public event Action<CardInstance> Added;
		public event Action<CardInstance> Removed;
		public event Action<CardPile> Changed;

#endregion

		public CardInstance Last => Cards[Cards.Count - 1];
		public CardInstance First => Cards[0];

		public List<CardInstance> Cards
		{
			get => m_cards;
			set
			{
				m_cards = value;
				Changed?.Invoke(this);
			}
		}

		public int Count => Cards.Count;

		[SerializeField]
		private List<CardInstance> m_cards;

		public CardPile()
		{
			Cards = new List<CardInstance>();
		}

		public CardPile(List<CardInstance> cards)
		{
			Cards = cards;
		}

		public virtual void Add(CardInstance card)
		{
			Cards.Add(card);
			Added?.Invoke(card);
		}

		public virtual void Remove(CardInstance card)
		{
			if (Cards.Contains(card))
			{
				Cards.Remove(card);
				Removed?.Invoke(card);
			}
		}

		/// <summary>
		/// Resets the Pile.
		/// </summary>
		public void Clear()
		{
			Cards = new List<CardInstance>();
			Changed?.Invoke(this);
		}

		/// <summary>
		/// Finds all matching objects based on specification.
		/// </summary>
		/// <param name="specification">specify the filter</param>
		/// <returns>Matching results</returns>
		public IEnumerable<CardInstance> GetCollection(Func<CardData, bool> specification)
		{
			var filter = new CardInstanceFilter();

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
		public CardInstance GetSingle(Func<CardData, bool> specification)
		{
			var filter = new CardInstanceFilter();

			return filter.Single(Cards, specification);
		}
	}
}