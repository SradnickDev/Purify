using Cards.General;
using MessagePack;
using Misc.Events;
using Status.General;
using Units.General;
using UnityEngine;

namespace Status.Types
{
	[MessagePackObject(true)]
	public class ResilienceData : StatusData
	{
		public int Percentage;

		public ResilienceData()
		{
			Name = "Resilience";
		}

		public override StatusBase Initialize(Unit owner)
		{
			return new Resilience(this, owner);
		}
	}

	public class Resilience : TriggeredStatus
	{
		private int m_previousHealth;
		public Resilience(StatusData statusData, Unit unit) : base(statusData, unit) { }

		public override bool IsFinished => false;

		public override void SetupTrigger()
		{
			StatusData.Trigger.Requirement = new TurnStart() {Key = AffectedUnit.name};
		}

		public override void Activate()
		{
			base.Activate();
			m_previousHealth = (int) AffectedUnit.Health.Current;
		}

		public override void OnTriggerRaised()
		{
			var diff = m_previousHealth - AffectedUnit.Health.Current;

			var percentage = ((ResilienceData) StatusData).Percentage / 100f;
			percentage *= Instances;
			var amount = Mathf.FloorToInt(diff * percentage);
			if (amount > 0)
			{
				AffectedUnit.ChangeBlock(amount);
			}

			m_previousHealth = (int) AffectedUnit.Health.Current;
		}
	}
}