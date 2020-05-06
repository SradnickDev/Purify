using Cards.Effects.General;
using Cards.General;
using MessagePack;
using Units.General;
using Units.Player.General;

namespace Cards.Effects
{
	[MessagePackObject(true)]
	public class BanishWithCondition : Effect
	{
		public int Amount;
		private int m_counter = 0;

		public override void Execute(Unit target, Unit from)
		{
			if (from is Player player)
			{
				m_counter++;
				if (m_counter >= UseLens(@from, null, Amount))
				{
					player.Hand.DiscardPileType = DiscardPileType.BanishPile;

					m_counter = 0;
				}
			}
		}

		public override object Value(Unit @from, Unit target) => UseLens(@from, null, Amount);
	}
}