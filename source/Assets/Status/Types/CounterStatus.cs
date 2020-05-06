using MessagePack;
using Status.General;
using Units.General;

namespace Status.Types
{
	[MessagePackObject(true)]
	public abstract class CounterStatus : StatusBase
	{
		/// <summary>
		/// If true the Status will be deleted in StatusPhase.
		/// </summary>
		public override bool IsFinished => Duration <= 0;

		protected int Duration = 1;
		public override int Stacks => Duration;

		protected CounterStatus(StatusData statusData, Unit unit) : base(statusData, unit) { }

		/// <summary>
		/// Adds the Amount to the current Duration.
		/// Can be used to implement a unique behaviour.
		/// </summary>
		public override void AddStacks(int amount)
		{
			Duration += amount;
		}

		/// <summary>
		/// Call every StatusPhase of the Affected Unit.
		/// Can be used to implement a unique behaviour.
		/// </summary>
		public override void Update()
		{
			Duration--;
		}

		/// <summary>
		/// Is used to show the given string to the UI.
		/// </summary>
		/// <returns></returns>
		public override string ToString() => Duration.ToString();
	}
}