using Cards.Effects.General;
using Cards.General;
using MessagePack;
using Units.General;

namespace Cards.Effects
{
	[MessagePackObject(true)]
	public class DarkChant : Effect
	{
		public int Amount;

		protected override void DefineExecution(Unit target, Unit from, TargetType targetType)
		{
			base.DefineExecution(target, from, TargetType.AllEnemies);
		}

		public override void Execute(Unit target, Unit from)
		{
			target.Might.Current += Amount;
		}

		public override object Value(Unit @from, Unit target) => Amount;
	}
}