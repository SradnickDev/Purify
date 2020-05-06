using MessagePack;
using Status.General;
using Units.General;

namespace Status.Types
{
	[MessagePackObject(true)]
	public class UnholyPerseveranceData : StatusData
	{
		public UnholyPerseveranceData()
		{
			Name = "Unholy Perseverance";
		}

		public override StatusBase Initialize(Unit owner)
		{
			return new UnholyPerseverance(this, owner);
		}
	}

	public class UnholyPerseverance : TriggeredStatus
	{
		private int m_currentCorruptionStacks;
		private int m_additionalPerseverance;
		public UnholyPerseverance(StatusData statusData, Unit unit) : base(statusData, unit) { }

		public override bool IsFinished => Instances == 0;

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
				AffectedUnit.Perseverance.Current -= m_additionalPerseverance;
				m_additionalPerseverance = corruptionStacks * Instances;
				AffectedUnit.Perseverance.Current += m_additionalPerseverance;
				m_currentCorruptionStacks = corruptionStacks;
			}
		}

		public override void Deactivate()
		{
			AffectedUnit.Soul.CurrentChanged -= OnTriggerRaised;
		}
	}
}