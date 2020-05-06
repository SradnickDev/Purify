using Cards.General;

namespace Deck
{
	[System.Serializable]
	public class DrawPile : CardPile
	{
		public bool HasCard() => Count > 0;

		/// <summary>
		/// Card from pile, will be deleted.
		/// </summary>
		/// <returns>random card</returns>
		public CardInstance Draw()
		{
			var targetCard = Cards[0];
			Remove(targetCard);
			return targetCard;
		}
	}
}