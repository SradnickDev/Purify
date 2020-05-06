using Cards.Effects.General;
using MessagePack;
using Units.General;

namespace Cards.Effects
{
	[MessagePackObject(true)]
	public class RepeatEffect : Effect
	{
		public int Amount;
		public Effect Effect;

		public override void Execute(Unit target, Unit from)
		{
			if (Effect == null) return;
			for (var i = 0; i < UseLens(@from, null, Amount); i++)
			{
				Effect.Execute(target, from);
			}
		}

		public override object Value(Unit @from, Unit target)
		{
			var repeatAmount = UseLens(@from, null, Amount);
			var amount = Effect.Value(@from, target);
			var retVal = repeatAmount >= 1 ? $"{repeatAmount} x {amount}" : $"{repeatAmount}";
			return retVal;
		}
	}
}