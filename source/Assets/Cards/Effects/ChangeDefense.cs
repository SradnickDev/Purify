using Cards.Effects.General;
using Cards.General;
using MessagePack;
using Units.General;
using UnityEngine;
using Utilities;

namespace Cards.Effects
{
	[MessagePackObject(true)]
	public class ChangeDefense : Effect
	{
		public int Defense;
		public CustomTarget CustomTarget;

		public override void Execute(Unit target, Unit from)
		{
			if (CustomTarget == CustomTarget.SpecifiedTarget)
			{
				target.ChangeBlock(UseLens(target, null,Defense),false);
			}
			else
			{
				from.ChangeBlock(UseLens(@from, null,Defense),false);
			}
		}

		public override object Value(Unit @from, Unit target)
		{
			var value =  UseLens(@from, null, Defense);
			return GeneralUtilities.FormatValue(Defense, value, Color.red, new Color(0f, 0.4f, 0.05f));
		}
	}
}