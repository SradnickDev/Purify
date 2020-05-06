using MessagePack;
using Status.General;
using Units.General;
using UnityEngine;

namespace Status.Types
{
	[MessagePackObject(true)]
	public class DemonicInfusionData : StatusData
	{
		public DemonicInfusionData()
		{
			Name = "Demonic Infusion";
		}

		public override StatusBase Initialize(Unit owner)
		{
			return new DemonicInfusion(this, owner);
		}
	}

	public class DemonicInfusion : CounterStatus
	{
		public DemonicInfusion(StatusData statusData, Unit unit) : base(statusData, unit) { }

		public override void Update()
		{
			AffectedUnit.ChangeSoul(-Duration);
			Duration--;
		}
	}
}