using MessagePack;
using Misc.Events;
using Status.General;
using Units.General;
using UnityEngine;

namespace Status.Types
{
	[MessagePackObject(true)]
	public class MasochistData : StatusData
	{
		public int Amount;

		public MasochistData()
		{
			Name = "Masochist";
		}

		public override StatusBase Initialize(Unit owner)
		{
			return new Masochist(this, owner);
		}
	}

	public class Masochist : TriggeredStatus
	{
		public Masochist(StatusData statusData, Unit unit) : base(statusData, unit) { }

		public override bool IsFinished => false;

		public override void SetupTrigger()
		{
			StatusData.Trigger.Requirement = new Damaged(AffectedUnit.name);
		}

		public override void OnTriggerRaised()
		{
			for (var i = 0; i < Mathf.Max(Instances, 1); i++)
			{
				AffectedUnit.Soul.Current += ((MasochistData) StatusData).Amount;
			}
		}
	}
}