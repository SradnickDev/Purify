using System.Collections.Generic;
using MessagePack;

namespace Deck
{
	/// <summary>
	/// To store card ids.
	/// </summary>
	[MessagePackObject(true)]
	public class DeckSaveData
	{
		public List<CardSaveData> Cards = new List<CardSaveData>();
	}

	[MessagePackObject(true)]
	public class CardSaveData
	{
		public int CardId;
		public int Count;
	}
}