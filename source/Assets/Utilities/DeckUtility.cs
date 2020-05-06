using System.Collections.Generic;
using System.Linq;
using Battle.General;
using Cards.General;
using Deck;
using OptionMenu;
using UnityEngine;

namespace Utilities
{
	public static class DeckUtility
	{
#region PoolLoading

		public static CardPool LoadFixedStarterPool()
		{
			return LoadPool(Constants.FixedStarterCardPool);
		}

		public static CardPool LoadSelectionStarterPool()
		{
			return LoadPool(Constants.SelectionStarterPool);
		}

		public static CardPool LoadChosenPool()
		{
			return LoadPool("CardPools/" + Constants.ChosenPool);
		}

		public static CardPool LoadHiddenPool()
		{
			return LoadPool(Constants.HiddenCardPool);
		}

		public static IEnumerable<CardPool> LoadAllPools()
		{
			var pools = Resources.LoadAll<TextAsset>("CardPools/");

			foreach (var poolFile in pools)
			{
				if (poolFile.name == Constants.HiddenCardPoolName) continue;
				yield return PersistentJson.Create<CardPool>(poolFile.text);
			}
		}

		private static CardPool LoadPool(string path)
		{
			var loadedPool = Resources.Load<TextAsset>(path);
			var pool = PersistentJson.Create<CardPool>(loadedPool.text);
			return pool;
		}

#endregion

#region SaveData

		public static DeckSaveData LoadStarterDeckData()
		{
			var jsonFile = Resources.Load<TextAsset>(Constants.StarterDeckData);
			var data = PersistentJson.Create<DeckSaveData>(jsonFile.text);
			return data;
		}

		public static void SaveDeck(CardDeck deck)
		{
			var data = new DeckSaveData();

			foreach (var card in deck.Cards)
			{
				var id = card.CardData.Id;

				var index = data.Cards.FindIndex(x => x.CardId == id);

				if (index == -1)
				{
					data.Cards.Add(new CardSaveData() {CardId = id, Count = 1});
				}
				else
				{
					data.Cards[index].Count++;
				}
			}

			PersistentData.Save(data, Constants.PlayerDeckIdentifier);
		}

		public static DeckSaveData LoadSavedDeckData()
		{
			return PersistentData.Load<DeckSaveData>(Constants.PlayerDeckIdentifier);
		}

#endregion

#region Misc

		public static Sprite GetIconWithType(CardType type)
		{
			switch (type)
			{
				case CardType.Attack:
					return Resources.Load<Sprite>("Cards/Icons/IconAttack");
				case CardType.Defense:
					return Resources.Load<Sprite>("Cards/Icons/IconDefend");
				case CardType.Prayer:
					return Resources.Load<Sprite>("Cards/Icons/IconPrayer");
				case CardType.Blessing:
					return Resources.Load<Sprite>("Cards/Icons/IconBlessing");
			}

			return null;
		}

		/// <summary>
		/// Using Seed to retrieving a card based on pseudo random (deterministic) and on rarity.
		/// </summary>
		/// <param name="source">Cardpool that will be used. Will be cached and sorted.</param>
		public static CardData RarityBasedCard(CardPool source)
		{
			var data = new List<CardData>(source.Cards);
			var sortedList = data.OrderByDescending(x => x.Rarity).ToList();

			var sum = sortedList.Sum(cardData => (int) cardData.Rarity);


			var rndVal = RNG.Next(0, sum);

			foreach (var entry in sortedList)
			{
				if (rndVal < (int) entry.Rarity)
				{
					return entry;
				}

				rndVal -= (int) entry.Rarity;
			}

			return sortedList[sortedList.Count - 1];
		}

		/// <summary>
		/// Using Seed to retrieving a collection fo cards based on pseudo random (deterministic)and  on rarity.
		/// </summary>
		/// <param name="source">Cardpool that will be used. Will be cached and sorted.</param>
		public static IEnumerable<CardData> RarityBasedCards(CardPool source, int count)
		{
			var retVal = new List<CardData>();

			if (count > source.Cards.Count)
			{
				count = source.Cards.Count;
			}

			while (retVal.Count < count)
			{
				var card = RarityBasedCard(source);
				if (!retVal.Contains(card))
				{
					retVal.Add(card);
				}
			}

			return retVal;
		}

#endregion
	}
}