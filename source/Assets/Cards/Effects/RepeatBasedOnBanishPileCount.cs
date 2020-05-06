using Cards.Effects.General;
using MessagePack;
using Units.General;
using Units.Player.General;

namespace Cards.Effects
{
	[MessagePackObject(true)]
	public class RepeatBasedOnBanishPileCount : Effect
	{
		public Effect Effect = null;

		public override void Execute(Unit target, Unit from)
		{
			if (Effect == null) return;
			if (from is Player player)
			{
				var banishPile = player.BanishPile;

				var amount = banishPile.Count;

				for (var i = 0; i < amount; i++)
				{
					if (target == null) return;
					Effect.Execute(target, from);
				}
			}
		}

		public override object Value(Unit @from, Unit target) => Effect.ToString();
	}
}