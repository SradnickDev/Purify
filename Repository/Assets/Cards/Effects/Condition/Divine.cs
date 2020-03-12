using Cards.Effects.General;
using MessagePack;
using Units.General;
using Units.Player.General;

namespace Cards.Effects.Condition
{
	[MessagePackObject(true)]
	public class Divine : ConditionEffect
	{
		protected override bool DoEffect(Player player)
		{
			return player.Soul.PurityStacks(player.SoulStackThreshold) > 0;
		}

		public override object Value(Unit @from, Unit target) => "";
	}
}