using MessagePack;
using Status.General;
using Units.General;

namespace Status.Types
{
	[MessagePackObject(true)]
	public class DecrepitudeData : StatusData
	{
		public int Percentage;

		public DecrepitudeData()
		{
			Name = "Decrepitude";
		}

		public override StatusBase Initialize(Unit owner)
		{
			return new Decrepitude(this, owner);
		}
	}

	public class Decrepitude : CounterStatus
	{
		private DecrepitudeData m_decrepitudeData;
		public Decrepitude(StatusData statusData, Unit unit) : base(statusData, unit)
		{
			m_decrepitudeData = (DecrepitudeData) statusData;
		}
		

		public override void Activate()
		{
			AffectedUnit.Decrepitude -= m_decrepitudeData.Percentage / 100f;
		}

		public override void Deactivate()
		{
			AffectedUnit.Decrepitude += m_decrepitudeData.Percentage / 100f;
		}
	}
}