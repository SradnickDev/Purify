using MessagePack;
using Status.General;
using Units.General;

namespace Status.Types
{
	[MessagePackObject(true)]
	public class TemptationData : StatusData
	{
		public int Percentage;

		public TemptationData()
		{
			Name = "Temptation";
		}

		public override StatusBase Initialize(Unit owner)
		{
			return new Temptation(this, owner);
		}
	}

	public class Temptation : CounterStatus
	{
		private TemptationData m_temptationData;

		public Temptation(StatusData statusData, Unit unit) : base(statusData, unit)
		{
			m_temptationData = (TemptationData) statusData;
		}
		

		public override void Activate()
		{
			AffectedUnit.SoulMultiplier += m_temptationData.Percentage / 100f;
		}

		public override void Deactivate()
		{
			AffectedUnit.SoulMultiplier -= m_temptationData.Percentage / 100f;
		}
	}
}