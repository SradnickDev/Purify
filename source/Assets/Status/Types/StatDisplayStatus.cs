using Stats.General;
using Status.General;
using Units.General;

namespace Status.Types
{
	public abstract class StatDisplayStatus : StatusBase
	{
		protected AbstractStat ObservedStat;

		protected StatDisplayStatus(StatusData statusData, Unit unit) : base(statusData, unit) { }
		public override bool IsFinished => ObservedStat.Current == 0;

		public override int Stacks => ObservedStat.Current;

		public override void AddStacks(int amount) { }

		public override void Update() { }

		public override string ToString() =>
			ObservedStat == null ? "" : ObservedStat.Current.ToString();
	}
}