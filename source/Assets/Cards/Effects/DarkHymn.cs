using Cards.Effects.General;
using Cards.General;
using MessagePack;
using Units.General;

namespace Cards.Effects
{
	[MessagePackObject(true)]
	public class DarkHymn : Effect
	{
		public int Amount;

		protected override void DefineExecution(Unit target, Unit from, TargetType targetType)
		{
			base.DefineExecution(target, from, TargetType.AllEnemies);
		}

		public override void Execute(Unit target, Unit from)
		{
			target.Soul.Current += Amount;
		}

		public override object Value(Unit @from, Unit target) => Amount;
	}
}