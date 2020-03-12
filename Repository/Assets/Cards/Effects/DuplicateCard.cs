using Cards.Effects.General;
using Cards.General;
using MessagePack;
using Units.General;
using Units.Player.General;
using Utilities;

namespace Cards.Effects
{
	[MessagePackObject(true)]
	public class DuplicateCard : Effect
	{
		public CardType CardType = CardType.Attack;

		public override void Execute(Unit target, Unit from)
		{
			if (from is Player player)
			{
				var hand = player.Hand;
				var targetCard = hand.GetSingle(x => x.Type == CardType);

				if (targetCard != null)
				{
					var newData = GeneralExtensions.DeepCopy(targetCard.CardData);
					var copy = new CardInstance(newData, player);
					hand.Add(copy);
				}
			}
		}

		public override object Value(Unit @from, Unit target) => CardType.ToString();
	}
}