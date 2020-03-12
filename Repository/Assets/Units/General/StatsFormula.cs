using Stats.Types;
using UnityEngine;

namespace Units.General
{
	public static class StatsFormula
	{
		public static int CalculateDamage(float damage, Unit owner, float vulnerabilityMultiplier)
		{
			return Mathf.FloorToInt(CalculateDamage(damage, owner) * (1 + vulnerabilityMultiplier));
		}

		/// <summary>
		/// Damage based on Strength.
		/// </summary>
		public static float CalculateDamage(float damage, Unit owner)
		{
			return (damage + owner.Might.Current) * (1 + owner.Fatigue);
		}

		public static int CalculateDefense(int amount, Unit unit)
		{
			return Mathf.RoundToInt((amount + unit.Perseverance.Current) * (1 + unit.Decrepitude));
		}

		public static int CalculateSoul(int amount, Unit unit)
		{
			if (amount > 0)
			{
				return amount;
			}

			return Mathf.RoundToInt((amount * (1 + unit.SoulMultiplier)));
		}

		public static int PurityStacks(this Soul soul, int soulStackThreshold)
		{
			var stacks = RelativeSoulStacks(soul, soulStackThreshold);
			return soul.Current > 0 ? stacks : 0;
		}

		public static int CorruptionStacks(this Soul soul, int soulStackThreshold)
		{
			var stacks = RelativeSoulStacks(soul, soulStackThreshold);
			return soul.Current < 0 ? stacks : 0;
		}

		public static void AddPurityStacks(this Soul soul,
										   int amount,
										   int soulStackThreshold)
		{
			if (soul.Current > 0)
			{
				soul.Current += (soulStackThreshold * amount);
			}
		}

		public static void RemovePurityStacks(this Soul soul,
											  int amount,
											  int soulStackThreshold)
		{
			if (PurityStacks(soul, soulStackThreshold) >= amount)
			{
				soul.Current -= (soulStackThreshold * amount);
			}
		}

		public static void AddCorruptionStacks(this Soul soul,
											   int amount,
											   int soulStackThreshold)
		{
			if (soul.Current < 0)
			{
				soul.Current -= (soulStackThreshold * amount);
			}
		}

		public static void RemoveCorruptionStacks(this Soul soul,
												  int amount,
												  int soulStackThreshold)
		{
			if (CorruptionStacks(soul, soulStackThreshold) >= amount)
			{
				soul.Current += (soulStackThreshold * amount);
			}
		}

		private static int RelativeSoulStacks(Soul soul, int soulStackThreshold)
		{
			var value = Mathf.Abs(soul.Current);
			var stacks = Mathf.FloorToInt(value / soulStackThreshold);
			return stacks;
		}
	}
}