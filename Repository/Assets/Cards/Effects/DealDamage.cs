using Cards.Effects.General;
using Cards.General;
using MessagePack;
using Units.General;
using UnityEngine;
using Utilities;

namespace Cards.Effects
{
	[MessagePackObject(true)]
	public class DealDamage : Effect
	{
		public int Damage;
		public CustomTarget CustomTarget;

		public override void Execute(Unit target, Unit from)
		{
			if (CustomTarget == CustomTarget.SpecifiedTarget)
			{
				target.ApplyDamage(UseLens(from, target, Damage), from);
			}
			else
			{
				from.ApplyDamage(UseLens(from, from, Damage), from);
			}
		}

		public override object Value(Unit from, Unit target)
		{
			var value = UseLens(from, target, Damage);
			return GeneralUtilities.FormatValue(Damage, value, Color.red,
												new Color(0f, 0.4f, 0.05f));
		}
	}
}