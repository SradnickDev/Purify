using Cards.General;
using Misc.Events;
using Status;

namespace Deck
{
	public class BanishPile : CardPile
	{
		public override void Add(CardInstance card)
		{
			base.Add(card);
			EventLog.Add(new BanishCard());
		}
	}
}