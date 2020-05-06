using MessagePack;
using Status.General;
using Units.General;

namespace Status.Types
{
	[MessagePackObject(true)]
	public class CorruptionData : StatusData
	{
		public CorruptionData()
		{
			Name = "Corruption";
		}

		public override StatusBase Initialize(Unit owner)
		{
			return new CorruptionDisplayStatus(this, owner);
		}
	}

	public class CorruptionDisplayStatus : StatDisplayStatus
	{
		public CorruptionDisplayStatus(StatusData statusData, Unit unit) :
			base(statusData, unit) { }

		public override bool IsFinished => false;

		public override string ToString() => AffectedUnit
											 .Soul
											 .CorruptionStacks(AffectedUnit.SoulStackThreshold)
											 .ToString();
	}
}