using Battle.General;
using Cards.Effects.General;
using Cards.General;
using MessagePack;
using Units.General;
using Units.Player.General;

namespace Cards.Effects.Condition
{
	[MessagePackObject(true)]
	public class Retain : ConditionEffect
	{
		protected override bool DoEffect(Player player)
		{
			return BattleInfo.TurnPhase != TurnPhase.EndPhase;
		}

		public override object Value(Unit @from, Unit target) => "";
	}
}