using Cards.Effects.General;
using MessagePack;
using Units.General;
using Units.Player.General;

namespace Cards.Effects
{
	[MessagePackObject(true)]
	public class DiscardRandom : Effect
	{
		public int Amount;

		public override void Execute(Unit target, Unit from)
		{
			if (!(target is Player player)) return;

			for (var i = 0; i < UseLens(target, null, Amount); i++)
			{
				var card = player.Hand.GetRandomCard();
				if (card == null) break;
				player.Hand.Discard(card);
			}
		}

		public override object Value(Unit @from, Unit target) => UseLens(@from, null, Amount);
	}
}