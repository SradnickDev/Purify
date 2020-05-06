using System;
using MessagePack;
using Stats.General;
using UnityEngine;

namespace Stats.Types
{
	[Serializable] [MessagePackObject(true)]
	public sealed class Soul : AbstractStat
	{
		public event Action MinChanged;

		public int Min
		{
			set
			{
				m_min = value;
				MinChanged?.Invoke();
			}
			get => m_min;
		}

		[SerializeField] private int m_min = 0;

		public override void RemoveListener()
		{
			base.RemoveListener();
			MinChanged = null;
		}

		public override int Clamp(int current, int min, int max)
		{
			return base.Clamp(current, m_min, max);
		}

		public void Set(int min, int current, int max)
		{
			m_min = min;
			Set(current, max);
		}
	}
}