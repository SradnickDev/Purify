using Status.General;
using Units.General;

namespace Status.Types
{
	public abstract class StatusBase
	{
		public abstract bool IsFinished { get; }
		public readonly StatusData StatusData;
		public abstract int Stacks { get; }
		protected readonly Unit AffectedUnit;

		protected StatusBase(StatusData statusData, Unit unit)
		{
			StatusData = statusData;
			AffectedUnit = unit;
		}

		public abstract void AddStacks(int amount);
		public virtual void Activate() { }

		public abstract void Update();

		public virtual void Deactivate() { }

		public new abstract string ToString();
	}
}