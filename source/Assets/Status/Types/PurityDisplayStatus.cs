using MessagePack;
using Status.General;
using Units.General;

namespace Status.Types
{
	[MessagePackObject(true)]
	public class PurityData : StatusData
	{
		public PurityData()
		{
			Name = "Purity";
		}

		public override StatusBase Initialize(Unit owner)
		{
			return new PurityDisplayStatus(this, owner);
		}
	}

	public class PurityDisplayStatus : StatDisplayStatus
	{
		public PurityDisplayStatus(StatusData statusData, Unit unit) : base(statusData, unit) { }

		public override bool IsFinished => false;
		
		public override string ToString() => AffectedUnit
											 .Soul
											 .PurityStacks(AffectedUnit.SoulStackThreshold)
											 .ToString();
		
	}
}