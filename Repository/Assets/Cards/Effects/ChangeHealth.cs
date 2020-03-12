using Cards.Effects.General;
using Cards.General;
using MessagePack;
using Units.General;
using UnityEngine;

namespace Cards.Effects
{
	[MessagePackObject(true)]
	public class ChangeHealth : Effect
	{
		public int Amount;
		public CustomTarget CustomTarget;

		public override void Execute(Unit target, Unit from)
		{
			if (CustomTarget == CustomTarget.SpecifiedTarget)
			{
				target.Health.Current += UseLens(@from, null, Amount);
			}
			else
			{
				from.Health.Current += UseLens(@from, null, Amount);
			}
		}

		public override object Value(Unit @from, Unit target) => Mathf.Abs(UseLens(@from, null, Amount));
	}
}