using Cards.General;
using Units.General;
using Utilities;

namespace Deck
{
	public static class DeckFactory
	{
		/// <summary>
		/// Builds a starter deck or a specific one based on save data.
		/// </summary>
		/// <param name="saveData">Contains information about a saved deck</param>
		/// <param name="player">Needs to be set to have a proper Card Description</param>
		/// <returns>ready to use card deck</returns>
		public static CardDeck Build(DeckSaveData saveData, Unit player)
		{
			if (saveData != null)
			{
				return CreateDeckFromFile(saveData, player);
			}

			return CreateDefault(player);
		}

		/// <summary>
		/// Builds a starter deck or a specific one based on save data.
		/// </summary>
		/// <param name="saveData">Contains information about a saved deck</param>
		/// <returns>ready to use card deck</returns>
		public static CardDeck Build(DeckSaveData saveData)
		{
			if (saveData != null)
			{
				return CreateDeckFromFile(saveData, null);
			}

			//Fallback create pool from default deck data
			return CreateDefault(null);
		}

		private static CardDeck CreateDefault(Unit player)
		{
			var data = DeckUtility.LoadStarterDeckData();

			return CreateDeckFromFile(data, player);
		}

		/// <summary>
		/// Restores a deck from data.
		/// Loads all pools to find the correct card.
		/// </summary>
		/// <param name="saveData">save file</param>
		/// <param name="player">Every card is initialized with a Unit to create a card description based on stats</param>
		/// <returns>saved deck</returns>
		private static CardDeck CreateDeckFromFile(DeckSaveData saveData, Unit player)
		{
			var retVal = new CardDeck();

			var pools = DeckUtility.LoadAllPools();

			foreach (var pool in pools)
			{
				foreach (var cardData in saveData.Cards)
				{
					var data = pool.GetSingle(info =>
												  info.Id == cardData.CardId);

					for (var i = 0; i < cardData.Count; i++)
					{
						if (data == null) continue;

						var isNewId = retVal.Cards.Find(x => x.CardData.Id == data.Id) == null;

						if (isNewId)
						{
							retVal.Add(new CardInstance(data, player));
						}
						else
						{
							var newData = GeneralExtensions.DeepCopy(data);
							newData.Illustration = data.Illustration;
							newData.Icon = data.Icon;
							retVal.Add(new CardInstance(newData, player));
						}
					}
				}
			}

			return retVal;
		}

		/// <summary>
		/// WARNING , HEAVY LOAD! This Load all Pools again, shouldn't used often !  
		/// </summary>
		/// <param name="id">Card id</param>
		/// <returns></returns>
		private static CardData LoadSingleData(int id)
		{
			var pools = DeckUtility.LoadAllPools();

			foreach (var pool in pools)
			{
				var data = pool.GetSingle(info =>
											  info.Id == id);

				if (data != null)
				{
					return data;
				}
			}

			return null;
		}
	}
}