using Cards.General;
using MessagePack;
using Misc.Events;
using Status.General;
using Units.General;

namespace Status.Types
{
	[MessagePackObject(true)]
	public class RiposteData : StatusData
	{
		public int Amount;

		public RiposteData()
		{
			Name = "Riposte";
		}

		public override StatusBase Initialize(Unit owner)
		{
			return new Riposte(this, owner);
		}
	}

	public class Riposte : TriggeredStatus
	{
		private TriggeredAction m_removeTrigger;
		public Riposte(StatusData statusData, Unit unit) : base(statusData, unit) { }

		public override bool IsFinished => Instances == 0;

		public override void SetupTrigger()
		{
			m_removeTrigger = new TriggeredAction
			{
				Requirement = new TurnStart(AffectedUnit.name),
				OnTriggered = Remove
			};

			EventLog.Register(m_removeTrigger);
			StatusData.Trigger.Requirement = new Attacked(AffectedUnit.name);
		}

		public override void OnTriggerRaised()
		{
			DealDamage();

			Instances--;

			if (Instances == 0)
			{
				Remove();
			}
		}

		private void DealDamage()
		{
			var target = AffectedUnit.LastHit.Item1;
			target.ApplyDamage(((RiposteData) StatusData).Amount, AffectedUnit);
		}

		public void Remove()
		{
			AffectedUnit.StatusContainer.Remove(this);
		}

		public override void Deactivate()
		{
			EventLog.Deregister(m_removeTrigger);
			m_removeTrigger.Requirement = null;
			m_removeTrigger.OnTriggered = null;
			m_removeTrigger = null;
			base.Deactivate();
		}
	}
}