using Cards.Effects.General;
using Cards.General;
using MessagePack;
using Status.General;
using Status.Types;
using Units.General;

namespace Cards.Effects
{
	[MessagePackObject(true)]
	public class MultiplyStatusEffect : Effect
	{
		public int Multiplier;
		public StatusData TargetStatus;
		public CustomTarget CustomTarget;

		public override void Execute(Unit target, Unit from)
		{
			switch (CustomTarget)
			{
				case CustomTarget.Self:
					FindStatus(from);
					break;
				case CustomTarget.SpecifiedTarget:
					FindStatus(target);
					break;
			}
		}

		private void FindStatus(Unit target)
		{
			for (int i = 0; i < target.StatusContainer.Count; i++)
			{
				var status = target.StatusContainer[i];
				if (status.StatusData.GetType() == TargetStatus.GetType())
				{
					MultiplyValidMatch(status);
				}
			}
		}

		private void MultiplyValidMatch(StatusBase status)
		{
			if (int.TryParse(status.ToString(), out var stacks))
			{
				var amount = (stacks * Multiplier) - stacks;
				status.AddStacks(amount);
			}
		}

		public override object Value(Unit @from, Unit target) => Multiplier;
	}
}