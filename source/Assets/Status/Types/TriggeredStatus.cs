using Misc.Events;
using Status.General;
using Units.General;

namespace Status.Types
{
	public abstract class TriggeredStatus : StatusBase
	{
		protected bool IsTriggered = false;
		protected int Instances = 1;
		public override int Stacks => Instances;

		public TriggeredStatus(StatusData statusData, Unit unit) : base(statusData, unit) { }

		/// <summary>
		/// If true the Status will be deleted in StatusPhase.
		/// </summary>
		public override bool IsFinished => Instances == 0 && IsTriggered;

		/// <summary>
		/// Adds the Amount to the current Instances.
		/// Can be used to implement a unique behaviour.
		/// </summary>
		public override void AddStacks(int amount)
		{
			Instances += amount;
		}

		/// <summary>
		/// Called after applying to a unit. Setup the Trigger and register it to the EventLog.
		/// Can be overriden to create a unique or additional behaviour.
		/// </summary>
		public override void Activate()
		{
			StatusData.Trigger = new TriggeredAction();
			SetupTrigger();
			StatusData.Trigger.OnTriggered = OnTriggerRaised;
			EventLog.Register(StatusData.Trigger);
		}

		/// <summary>
		/// Called every Status Phase of the Affected Unit.
		/// Can be ignored since Triggered Status not contain any logic, use Counter Status,
		/// to implement a Status that depends on this.
		/// </summary>
		public override void Update() { }

		public abstract void SetupTrigger();

		/// <summary>
		/// Called if the Trigger is raised. Used to Implement unique logic,
		/// that happens if specific event is raised.
		/// </summary>
		public abstract void OnTriggerRaised();

		
		/// <summary>
		/// Called if the buff is deleted from the Affected Unit.
		/// Trigger will be de-registered from EventLog and deleted.
		/// </summary>
		public override void Deactivate()
		{
			EventLog.Deregister(StatusData.Trigger);
			StatusData.Trigger.OnTriggered = null;
			StatusData.Trigger = null;
		}

		public override string ToString() => Instances.ToString();
	}
}