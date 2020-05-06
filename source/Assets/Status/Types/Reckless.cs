using System.Linq;
using Cards.Effects;
using Cards.General;
using MessagePack;
using Status.General;
using Units.General;
using Units.Player.General;

namespace Status.Types
{
	[MessagePackObject(true)]
	public class RecklessData : StatusData
	{
		public RecklessData()
		{
			Name = "Reckless";
		}

		public override StatusBase Initialize(Unit owner)
		{
			return new Reckless(this, owner);
		}
	}

	public class Reckless : CounterStatus
	{
		public Reckless(StatusData statusData, Unit unit) : base(statusData, unit) { }

		public override bool IsFinished => false;

		public override void AddStacks(int amount) { }

		public override void Activate()
		{
			ModifyAttackCards();
		}

		private void ModifyAttackCards()
		{
			if (AffectedUnit is Player player)
			{
				var hand = player.Hand.GetCollection(x => x.Type == CardType.Attack);
				foreach (var card in hand)
				{
					ApplyCardChanges(card);
				}

				var drawPile = player.Hand.GetCollection(x => x.Type == CardType.Attack);
				foreach (var card in drawPile)
				{
					ApplyCardChanges(card);
				}

				var discardPile = player.Hand.GetCollection(x => x.Type == CardType.Attack);
				foreach (var card in discardPile)
				{
					ApplyCardChanges(card);
				}
			}
		}

		private static void ApplyCardChanges(CardInstance card)
		{
			card.CardData.Energy = 0;
			card.CardData.PlayEffect.Add(new BanishWithCondition() {Amount = 1});
		}

		public override string ToString() => "";

		public override void Deactivate() { }
	}
}