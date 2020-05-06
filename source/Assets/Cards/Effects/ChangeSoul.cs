using Cards.Effects.General;
using Cards.General;
using MessagePack;
using Stats.Types;
using Units.General;
using UnityEngine;

namespace Cards.Effects
{
	[MessagePackObject(true)]
	public class ChangeSoul : Effect
	{
		public int Soul;
		public CustomTarget CustomTarget;

		public override void Execute(Unit target, Unit from)
		{
			if (CustomTarget == CustomTarget.SpecifiedTarget)
			{
				target.ChangeSoul(UseLens(target, null, Soul), false);
			}
			else
			{
				from.ChangeSoul(UseLens(@from, null, Soul), false);
			}
		}

		public override object Value(Unit @from, Unit target) => Mathf.Abs(UseLens(@from, null, Soul));
	}
}