using MessagePack;
using Status.General;
using Units.General;

namespace Status.Types
{
	[MessagePackObject(true)]
	public class MightData : StatusData
	{
		public MightData()
		{
			Name = "Might";
		}

		public override StatusBase Initialize(Unit owner)
		{
			return new MightDisplayStatus(this, owner);
		}
	}

	public class MightDisplayStatus : StatDisplayStatus
	{
		public MightDisplayStatus(StatusData statusData, Unit unit) : base(statusData, unit)
		{
			ObservedStat = unit.Might;
		}
		public override bool IsFinished => false;
	}
}