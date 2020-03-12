using Cards.General;
using MessagePack;
using Misc.Events;
using Status.General;
using Units.General;

namespace Status.Types
{
	[MessagePackObject(true)]
	public class DarkBlessingData : StatusData
	{
		public int Might;
		public int Corruption;

		public DarkBlessingData()
		{
			Name = "Dark Blessing";
		}

		public override StatusBase Initialize(Unit owner)
		{
			return new DarkBlessing(this, owner);
		}
	}

	public class DarkBlessing : TriggeredStatus
	{
		public DarkBlessing(StatusData statusData, Unit unit) : base(statusData, unit) { }

		public override bool IsFinished => false;

		public override void SetupTrigger()
		{
			StatusData.Trigger.Requirement = new TurnStart() {Key = AffectedUnit.name};
		}

		public override void OnTriggerRaised()
		{
			ApplyMight();
			ApplyCorruption();
		}

		private void ApplyCorruption()
		{
			var corruption = -(((DarkBlessingData) StatusData).Corruption * Instances);
			AffectedUnit.ChangeSoul(corruption);
		}

		private void ApplyMight()
		{
			var might = ((DarkBlessingData) StatusData).Might * Instances;
			AffectedUnit.Might.Current += might;
		}
	}
}