using MessagePack;
using Status.General;
using Units.General;

namespace Status.Types
{
	[MessagePackObject(true)]
	public class PerseveranceData : StatusData
	{
		public PerseveranceData()
		{
			Name = "Perseverance";
		}

		public override StatusBase Initialize(Unit owner)
		{
			return new PerseveranceDisplayStatus(this, owner);
		}
	}

	public class PerseveranceDisplayStatus : StatDisplayStatus
	{
		public PerseveranceDisplayStatus(StatusData statusData, Unit unit) : base(statusData, unit)
		{
			ObservedStat = unit.Perseverance;
		}

		public override bool IsFinished => false;
	}
}