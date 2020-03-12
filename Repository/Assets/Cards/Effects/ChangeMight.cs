using Cards.Effects.General;
using Cards.General;
using MessagePack;
using Units.General;

namespace Cards.Effects
{
	[MessagePackObject(true)]
	public class ChangeMight : Effect
	{
		public int Amount;
		public CustomTarget CustomTarget;

		public override void Execute(Unit target, Unit from)
		{
			switch (CustomTarget)
			{
				case CustomTarget.Self:
					from.Might.Current += UseLens(@from, null, Amount);
					break;
				case CustomTarget.SpecifiedTarget:
					target.Might.Current += UseLens(@from, null, Amount);
					break;
			}
		}

		public override object Value(Unit @from, Unit target) => UseLens(@from, null, Amount);
	}
}