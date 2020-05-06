using Cards.Effects.General;
using MessagePack;
using Units.General;
using Units.Player.General;
using UnityEngine;

namespace Cards.Effects
{
	[MessagePackObject(true)]
	public class DrawCard : Effect
	{
		public int Amount;

		public override void Execute(Unit target, Unit from)
		{
			if (from is Player player)
			{
				for (var i = 0; i < UseLens(@from, null, Amount); i++)
				{
					player.DrawCard();
				}
			}
		}

		public override object Value(Unit @from, Unit target) => UseLens(@from, null, Amount);
	}
}