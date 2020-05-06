using MessagePack;
using Status.General;
using Units.General;

namespace Status.Types
{
	[MessagePackObject(true)]
	public class UnholyMightData : StatusData
	{
		public UnholyMightData()
		{
			Name = "Unholy Might";
		}

		public override StatusBase Initialize(Unit owner)
		{
			return new UnholyMight(this, owner);
		}
	}

	public class UnholyMight : TriggeredStatus
	{
		private int m_currentCorruptionStacks;
		private int m_additionalMight;
		public UnholyMight(StatusData statusData, Unit unit) : base(statusData, unit) { }

		public override bool IsFinished => false;

		public override void AddStacks(int amount)
		{
			base.AddStacks(amount);
			OnTriggerRaised();
		}

		public override void Activate()
		{
			SetupTrigger();
		}

		public override void SetupTrigger()
		{
			AffectedUnit.Soul.CurrentChanged += OnTriggerRaised;
			OnTriggerRaised();
		}

		public override void OnTriggerRaised()
		{
			var corruptionStacks =
				AffectedUnit.Soul.CorruptionStacks(AffectedUnit.SoulStackThreshold);

			if (m_currentCorruptionStacks != corruptionStacks ||
				m_currentCorruptionStacks == 0)
			{
				AffectedUnit.Might.Current -= m_additionalMight;
				m_additionalMight = corruptionStacks * Instances;
				AffectedUnit.Might.Current += m_additionalMight;
				m_currentCorruptionStacks = corruptionStacks;
			}
		}

		public override void Deactivate()
		{
			AffectedUnit.Soul.CurrentChanged -= OnTriggerRaised;
		}
	}
}