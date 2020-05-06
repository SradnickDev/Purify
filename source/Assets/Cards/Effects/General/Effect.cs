using Battle.General;
using Cards.General;
using MessagePack;
using Units.General;

namespace Cards.Effects.General
{
#region Attributes

	[Union(0, typeof(ChangeHealth))] [Union(1, typeof(ChangeMight))]
	[Union(2, typeof(ChangePerseverance))] [Union(3, typeof(ChangeEnemyIntent))]
	[Union(4, typeof(ApplyStatusEffect))] [Union(5, typeof(BanishWithCondition))]
	[Union(6, typeof(ChangeDefense))] [Union(7, typeof(ChangeSoul))]
	[Union(8, typeof(DealDamage))] [Union(9, typeof(DiscardRandom))]
	[Union(10, typeof(DrawCard))] [Union(11, typeof(DuplicateCard))]
	[Union(12, typeof(RepeatBasedOnBanishPileCount))] [Union(13, typeof(RepeatEffect))]
	[Union(15, typeof(MultiplyStatusEffect))] [Union(16, typeof(DarkChant))]
	[Union(17, typeof(DemonSpawn))] [Union(18, typeof(DarkHymn))]
	[MessagePackObject(true)]

#endregion

	public abstract class Effect : IDescriptionValue
	{
		public Lens Lens = null;

		/// <summary>
		/// Called to execute the target Effect.
		/// </summary>
		/// <param name="target">Possible Target</param>
		/// <param name="from">Effect Origin / Owner</param>
		/// <param name="targetType">Defines on which group the effect will be executed.</param>
		public void Use(Unit target, Unit from, TargetType targetType)
		{
			DefineExecution(target, from, targetType);
		}

		/// <summary>
		/// Can be overriden to implement and own behaviour. Default using BattleInfo to solve target selection. 
		/// </summary>
		/// <param name="target"></param>
		/// <param name="from"></param>
		/// <param name="targetType"></param>
		protected virtual void DefineExecution(Unit target, Unit from, TargetType targetType)
		{
			BattleInfo.SolveTargetSelection(target, targetType, from, Execute);
		}

		/// <summary>
		/// Contains effect code.
		/// </summary>
		/// <param name="target">Target on which the effect can ber applied.</param>
		/// <param name="from">Owner needed to e.g calculate specific. Can also be used as a target. </param>
		public abstract void Execute(Unit target, Unit from);

		/// <summary>
		/// Using a lens to calculate Values.
		/// </summary>
		/// <param name="unit">From / owner</param>
		/// <param name="target">Target can be needed for specific calculation</param>
		/// <param name="value">Origin value that will used.</param>
		/// <returns></returns>
		protected int UseLens(Unit unit, Unit target, int value)
		{
			if (unit == null || Lens == null)
			{
				return value;
			}

			return Lens.Calculate(unit, target, value);
		}

		/// <summary>
		/// Used to display values on Cards.
		/// </summary>
		public abstract object Value(Unit from, Unit target);
	}
}