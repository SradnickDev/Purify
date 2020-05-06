using System;
using System.Collections.Generic;
using System.Linq;
using MessagePack;
using Stats.Types;
using UnityEngine;

namespace Stats.General
{
	/// <summary>
	/// Base class for a Stat.
	/// Provides Min,Max and Current value
	/// Current Value will be clamped between min and max.
	/// </summary>
	[Union(0, typeof(Health))] [Union(1, typeof(Defense))]
	[Union(2, typeof(Energy))] [Union(3, typeof(Might))]
	[Union(4, typeof(Perseverance))] [Union(5, typeof(Soul))]
	[Serializable] [MessagePackObject(true)]
	public abstract class AbstractStat
	{
		public event Action CurrentChanged;
		public event Action MaxChanged;

		public int Max
		{
			set
			{
				m_max = value;
				MaxChanged?.Invoke();
			}
			get => m_max;
		}

		public int Current
		{
			set
			{
				m_current = Clamp(value, 0, Max);
				CurrentChanged?.Invoke();
			}
			get => m_current;
		}

		[SerializeField] private int m_max = 0;
		[SerializeField] private int m_current = 0;

		public AbstractStat()
		{
			Current = Max;
		}

		public AbstractStat(int newCurrent)
		{
			Current = newCurrent;
		}

		public void SetDirty()
		{
			CurrentChanged?.Invoke();
			MaxChanged?.Invoke();
		}

		public virtual void RemoveListener()
		{
			CurrentChanged = null;
			MaxChanged = null;
		}

		public virtual int Clamp(int current, int min, int max)
		{
			return Mathf.Clamp(current, min, max);
		}

		/// <summary>
		/// Set values without trigger Events.
		/// </summary>
		public virtual void Set(int current, int max)
		{
			m_current = current;
			m_max = max;
		}

		public override string ToString()
		{
			return m_current.ToString();
		}
	}
}