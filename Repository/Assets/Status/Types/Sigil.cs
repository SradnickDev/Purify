using Cards.General;
using MessagePack;
using Misc.Events;
using Status.General;
using Units.General;

namespace Status.Types
{
	[MessagePackObject(true)]
	public class SigilData : StatusData
	{
		public SigilData()
		{
			Name = "Sigil";
		}

		public override StatusBase Initialize(Unit owner)
		{
			return new Sigil(this, owner);
		}
	}

	public class Sigil : TriggeredStatus
	{
		public Sigil(StatusData statusData, Unit unit) : base(statusData, unit) { }

		public override bool IsFinished => Instances == 0;

		public override void SetupTrigger()
		{
			StatusData.Trigger.Requirement = new Debuffed(AffectedUnit.name);
		}

		public override void OnTriggerRaised()
		{
			RemoveOneStatusFromTypeDebuff();

			Instances--;

			if (Instances == 0)
			{
				AffectedUnit.StatusContainer.Remove(this);
			}
		}

		private void RemoveOneStatusFromTypeDebuff()
		{
			var count = AffectedUnit.StatusContainer.Count;
			for (var i = AffectedUnit.StatusContainer.Count - 1; i >= 0; i--)
			{
				if (AffectedUnit.StatusContainer[i].StatusData.BuffType == BuffType.Debuff)
				{
					AffectedUnit.StatusContainer.Remove(AffectedUnit.StatusContainer[i]);
					break;
				}
			}
		}
	}
}