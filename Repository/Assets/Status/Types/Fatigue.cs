using MessagePack;
using Status.General;
using Units.General;
using UnityEngine;

namespace Status.Types
{
	[MessagePackObject(true)]
	public class FatigueData : StatusData
	{
		public int Percentage;

		public FatigueData()
		{
			Name = "Fatigue";
		}

		public override StatusBase Initialize(Unit owner)
		{
			return new Fatigue(this, owner);
		}
	}

	public class Fatigue : CounterStatus
	{
		private FatigueData m_fatigueData;

		public Fatigue(StatusData statusData, Unit unit) : base(statusData, unit)
		{
			m_fatigueData = (FatigueData) statusData;
		}

		public override void Activate()
		{
			AffectedUnit.Fatigue += -m_fatigueData.Percentage / 100f;
		}

		public override void Deactivate()
		{
			Debug.Log("asd");
			AffectedUnit.Fatigue -= -m_fatigueData.Percentage / 100f;
		}
	}
}