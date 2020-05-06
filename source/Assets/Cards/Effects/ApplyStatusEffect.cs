using Battle.General;
using Cards.Effects.General;
using Cards.General;
using MessagePack;
using Status.General;
using Units.General;
using UnityEngine;

namespace Cards.Effects
{
	[MessagePackObject(true)]
	public class ApplyStatusEffect : Effect
	{
		public CustomTarget CustomTarget;
		public int Count = 1;
		public StatusData Status;

		protected override void DefineExecution(Unit target, Unit from, TargetType targetType)
		{
			if (CustomTarget == CustomTarget.SpecifiedTarget)
			{
				BattleInfo.SolveTargetSelection(target, targetType, from, Execute);
			}

			if (CustomTarget == CustomTarget.Self)
			{
				Execute(from, from);
			}
		}

		public override void Execute(Unit target, Unit from)
		{
			AddToTarget(target);
		}

		private void AddToTarget(Unit target)
		{
			var status = Status.Initialize(target);
			status.AddStacks(Mathf.Max(Count - 1, 0));
			target.StatusContainer.Apply(status);
		}

		public override object Value(Unit @from, Unit target) => UseLens(@from, null, Count);
	}
}