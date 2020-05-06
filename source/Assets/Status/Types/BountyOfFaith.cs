using MessagePack;
using Misc.Events;
using Status.General;
using Units.General;

namespace Status.Types
{
	[MessagePackObject(true)]
	public class BountyOfFaithData : StatusData
	{
		public int Amount;

		public BountyOfFaithData()
		{
			Name = "Bounty Of Faith";
		}

		public override StatusBase Initialize(Unit owner)
		{
			return new BountyOfFaith(this, owner);
		}
	}

	public class BountyOfFaith : TriggeredStatus
	{
		public BountyOfFaith(StatusData statusData, Unit unit) : base(statusData, unit) { }

		public override bool IsFinished => false;

		public override void SetupTrigger()
		{
			StatusData.Trigger.Requirement = new PlayerWon();
		}

		public override void OnTriggerRaised()
		{
			for (var i = 0; i < Instances; i++)
			{
				AffectedUnit.Health.Current += ((BountyOfFaithData) StatusData).Amount;
			}
		}
	}
}