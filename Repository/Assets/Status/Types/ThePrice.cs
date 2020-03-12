using Cards.General;
using MessagePack;
using Microsoft.Win32.SafeHandles;
using Misc.Events;
using Status.General;
using Units.General;

namespace Status.Types
{
	[MessagePackObject(true)]
	public class ThePriceData : StatusData
	{
		public int Amount;

		public ThePriceData()
		{
			Name = "The Price";
		}

		public override StatusBase Initialize(Unit owner)
		{
			return new ThePrice(this, owner);
		}
	}

	public class ThePrice : TriggeredStatus
	{
		public ThePrice(StatusData statusData, Unit unit) : base(statusData, unit) { }

		public override bool IsFinished => false;
		public override void AddStacks(int amount) { }

		public override void SetupTrigger()
		{
			StatusData.Trigger.Requirement = new BanishCard();
		}

		public override void OnTriggerRaised()
		{
			AffectedUnit.ChangeSoul(-((ThePriceData) StatusData).Amount);
		}

		public override string ToString() => "";
	}
}