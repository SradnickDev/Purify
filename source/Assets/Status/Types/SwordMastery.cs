using Cards.General;
using MessagePack;
using Misc.Events;
using Status.General;
using Units.General;
using Units.Player.General;
using Utilities;

namespace Status.Types
{
	[MessagePackObject(true)]
	public class SwordMasteryData : StatusData
	{
		public int CardID;

		public SwordMasteryData()
		{
			Name = "Sword Mastery";
		}

		public override StatusBase Initialize(Unit owner)
		{
			return new SwordMastery(this, owner);
		}
	}

	public class SwordMastery : TriggeredStatus
	{
		private CardData m_afterBlowCardData;
		public SwordMastery(StatusData statusData, Unit unit) : base(statusData, unit) { }

		public override bool IsFinished => false;

		public override void SetupTrigger()
		{
			StatusData.Trigger.Requirement = new PlayedCardType(CardType.Attack);
		}

		public override void Activate()
		{
			LoadCardFromPool();
			base.Activate();
		}

		private void LoadCardFromPool()
		{
			var data = (SwordMasteryData) StatusData;
			var hiddenPool = DeckUtility.LoadHiddenPool();
			m_afterBlowCardData = hiddenPool.GetSingle(x => x.Id == data.CardID);
		}

		public override void OnTriggerRaised()
		{
			AddAfterBlowToHand();
		}

		private void AddAfterBlowToHand()
		{
			var data = (SwordMasteryData) StatusData;
			var player = (Player) AffectedUnit;
			var hand = player.Hand;

			if (player == null) return;
			//only add a new afterblow if the played attack card was not a afterblow card
			if (hand.LastPlayedCard.CardData.Id == data.CardID) return;

			for (var i = 0; i < Instances; i++)
			{
				var newData = GeneralExtensions.DeepCopy(m_afterBlowCardData);
				newData.Illustration = m_afterBlowCardData.Illustration;
				newData.Icon = data.Icon;
				hand.Add(new CardInstance(newData,player));
			}
		}
	}
}