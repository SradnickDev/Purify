using MessagePack;
using Misc.Events;
using Status.General;
using Units.General;

namespace Status.Types
{
	[MessagePackObject(true)]
	public class DivineInterventionData : StatusData
	{
		public DivineInterventionData()
		{
			Name = "Divine Intervention";
		}

		public override StatusBase Initialize(Unit owner)
		{
			return new DivineIntervention(this, owner);
		}
	}

	public class DivineIntervention : TriggeredStatus
	{
		private int m_previousHealth;
		public DivineIntervention(StatusData statusData, Unit unit) : base(statusData, unit) { }

		public override bool IsFinished => false;

		public override void SetupTrigger()
		{
			StatusData.Trigger.Requirement = new Damaged(AffectedUnit.name);
		}

		public override void Activate()
		{
			base.Activate();
			m_previousHealth = AffectedUnit.Health.Current;
		}

		public override void OnTriggerRaised()
		{
			AffectedUnit.Health.Current = m_previousHealth;
			m_previousHealth = AffectedUnit.Health.Current;

			Instances--;

			if (Instances == 0)
			{
				AffectedUnit.StatusContainer.Remove(this);
			}
		}
	}
}