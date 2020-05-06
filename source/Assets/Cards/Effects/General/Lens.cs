using System;
using System.Linq;
using Cards.General;
using MessagePack;
using Misc;
using Stats.General;
using Status;
using Units.General;
using Units.Player.General;
using UnityEngine;
using Utilities;

namespace Cards.Effects.General
{
	/// <summary>
	/// Lense wraps up specific ways how values are calcualted.
	/// Can be selected from a dropdown in custom editors.
	/// </summary>
	[Union(0, typeof(CalculateDamage))] [Union(1, typeof(CalculateDefense))]
	[Union(2, typeof(CalculateSoul))] [Union(3, typeof(CalculateBasedOnMissingHealth))]
	[Union(4, typeof(CalculateBasedOnStat))] [Union(5, typeof(CalculateBasedOnName))]
	[Union(6, typeof(CalculateBasedOnStack))] [Union(7, typeof(CalculateBasedOnPlayedCardType))]
	[MessagePackObject(true)]
	public abstract class Lens
	{
		public abstract int Calculate(Unit unit, Unit target, int value);
	}

	[MessagePackObject(true)]
	public class CalculateDamage : Lens
	{
		public override int Calculate(Unit unit, Unit target, int value)
		{
			if (target)
			{
				return StatsFormula.CalculateDamage(value, unit, target.VulnerabilityMultiplier);
			}

			return (int) StatsFormula.CalculateDamage(value, unit);
		}
	}

	[MessagePackObject(true)]
	public class CalculateDefense : Lens
	{
		public override int Calculate(Unit unit, Unit target, int value)
			=> StatsFormula.CalculateDefense(value, unit);
	}

	[MessagePackObject(true)]
	public class CalculateSoul : Lens
	{
		public override int Calculate(Unit unit, Unit target, int value)
			=> StatsFormula.CalculateSoul(value, unit);
	}

	[MessagePackObject(true)]
	public class CalculateBasedOnMissingHealth : Lens
	{
		public int Multiplier;
		public Lens Default;

		public override int Calculate(Unit unit, Unit target, int value)
		{
			var missingHealth = unit.Health.Max - unit.Health.Current;
			var amount = Mathf.FloorToInt(missingHealth * (Multiplier / 100f));
			if (unit == null && target == null)
			{
				return 0;
			}

			return Default?.Calculate(unit, target, amount) ?? amount;
		}
	}

	[MessagePackObject(true)]
	public class CalculateBasedOnStat : Lens
	{
		public int Multiplier;
		public AbstractStat TargetStat;
		public Lens Default;

		public override int Calculate(Unit unit, Unit target, int value)
		{
			var amount = 0;

			if (TargetStat != null)
			{
				amount = Mathf.FloorToInt(Mathf.Abs(unit.Stat(TargetStat.GetType()).Current *
													(Multiplier / 100f)));
			}

			return Default?.Calculate(unit, target, amount) ?? amount;
		}
	}

	[MessagePackObject(true)]
	public class CalculateBasedOnName : Lens
	{
		public string Name;
		public int Multiplier;
		public Lens Default;

		public override int Calculate(Unit unit, Unit target, int value)
		{
			var amount = value;

			if (unit is Player player)
			{
				var cardCount = CountCardsWith(Name, player);
				if (cardCount > 0)
				{
					amount += cardCount * Multiplier;
				}
			}
			else
			{
				Debug.LogWarning($"Player is needed. {this}");
			}

			return Default.Calculate(unit, target, amount);
		}

		private int CountCardsWith(string name, Player player)
		{
			bool Specification(CardData cardData)
			{
				var result =
					cardData.Name.IndexOf(name, StringComparison.CurrentCultureIgnoreCase) >=
					0;
				return result;
			}

			var countInHand = player.Hand.GetCollection(Specification).ToList().Count;
			var countInDrawPile = player.DrawPile.GetCollection(Specification).ToList().Count;
			var countInDiscardPile = player.DiscardPile.GetCollection(Specification).ToList().Count;
			var countInBanishPile = player.BanishPile.GetCollection(Specification).ToList().Count;

			var count = countInHand + countInDrawPile + countInDiscardPile + countInBanishPile;
			return count;
		}
	}

	[MessagePackObject(true)]
	public class CalculateBasedOnStack : Lens
	{
		public int Multiplier = 100;
		public bool UsePurity;
		public Lens Default;

		public override int Calculate(Unit unit, Unit target, int value)
		{
			var stackCount = UsePurity
				? unit.Soul.PurityStacks(unit.SoulStackThreshold)
				: unit.Soul.CorruptionStacks(unit.SoulStackThreshold);

			stackCount = Mathf.FloorToInt(stackCount * (Multiplier / 100f));
			var amount = value * stackCount;
			return Default?.Calculate(unit, target, amount) ?? amount;
		}
	}

	[MessagePackObject(true)]
	public class CalculateBasedOnPlayedCardType : Lens
	{
		public CardType CardType;
		public Lens Default;

		public override int Calculate(Unit unit, Unit target, int value)
		{
			var count = CardTurnLog.Count(CardType);
			var amount = value + count;
			return Default?.Calculate(unit, target, amount) ?? amount;
		}
	}
}