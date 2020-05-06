using Cards.Effects.General;
using MessagePack;
using Units.General;
using Units.Player.General;

namespace Cards.Effects.Condition
{
	[MessagePackObject(true)]
	public class Demonic: ConditionEffect
	{
		protected override bool DoEffect(Player player)
		{
			return player.Soul.CorruptionStacks(player.SoulStackThreshold) > 0;
		}
		
		public override object Value(Unit @from, Unit target) => "";
	}
}