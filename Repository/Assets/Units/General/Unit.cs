using System;
using Drawer;
using Misc.Events;
using Stats.General;
using Stats.Types;
using Status;
using Status.General;
using UnityEngine;
using Utilities;
using Logger = Utilities.Logger;

namespace Units.General
{
	public abstract class Unit : MonoBehaviour
	{
		[Header("Stats")]
		public Health Health;

		public Defense Defense;
		public Might Might;
		public Perseverance Perseverance;
		public Soul Soul;
		public int SoulStackThreshold = 10;

		[Space(10)]
		public StatusContainer StatusContainer = new StatusContainer();

		public Tuple<Unit, int> LastHit { get; private set; }
		[ReadOnly] public float Fatigue = 0;
		[ReadOnly] public float VulnerabilityMultiplier = 0;
		[ReadOnly] public float Decrepitude = 0;
		[ReadOnly] public float SoulMultiplier = 0;

		public virtual void Start()
		{
			StatusContainer.Setup(this);
		}

		public void ApplyDamage(int damage, Unit from, bool useDefaultFormula = false)
		{
			var actualDamage = useDefaultFormula
				? StatsFormula.CalculateDamage(damage, from, VulnerabilityMultiplier)
				: damage;

			Logger.Log("Received Damage", actualDamage + " from " + from);

			var blockDiff = Defense.Current - actualDamage;
			LastHit = new Tuple<Unit, int>(from, actualDamage);

			if (blockDiff < 0)
			{
				ApplyDamage(blockDiff, actualDamage);
			}
			else
			{
				BlockDamage(blockDiff);
			}

			EventLog.Add(new Attacked(name));
		}

		private void ApplyDamage(int blockDiff, int actualDamage)
		{
			Defense.Current = 0;
			var dmg = Mathf.Abs(blockDiff);
			Health.Current -= dmg;
			EventLog.Add(new Damaged(name));
		}

		private void BlockDamage(int blockDiff)
		{
			Defense.Current = blockDiff;
			EventLog.Add(new BlockedDamage(name));
		}

		public void Heal(int amount)
		{
			Health.Current += amount;
			EventLog.Add(new Healed(name));
		}

		public void ChangeBlock(int amount, bool useDefaultFormula = true)
		{
			var actualBlock = 0;
			actualBlock = useDefaultFormula
				? StatsFormula.CalculateDefense(amount, this)
				: amount;

			Defense.Current += actualBlock;

			EventLog.Add(new GetDefense(name));
		}

		public void ChangeSoul(int amount, bool useDefaultFormula = true)
		{
			var actualAmount = useDefaultFormula
				? StatsFormula.CalculateSoul(amount, this)
				: amount;

			if (amount > 0)
			{
				Purify(actualAmount);
			}
			else
			{
				Corrupt(actualAmount);
			}
		}

		private void Purify(int amount)
		{
			Soul.Current += amount;
			EventLog.Add(new Purified(name));
		}

		private void Corrupt(int amount)
		{
			Soul.Current += amount;
			EventLog.Add(new Corrupted(name));
		}

		private void OnDestroy()
		{
			Soul.RemoveListener();
			Defense.RemoveListener();
		}
	}
}