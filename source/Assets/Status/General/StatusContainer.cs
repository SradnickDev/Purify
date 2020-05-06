using System;
using System.Collections.Generic;
using System.Linq;
using Cards.General;
using Misc.Events;
using Stats.Types;
using Status.Types;
using Units.General;
using UnityEngine;
using Utilities;
using Logger = Utilities.Logger;

namespace Status.General
{
	public class StatusContainer
	{
		public event Action<StatusBase> Added;
		public event Action<StatusBase> Removed;

		private List<StatusBase> m_currentStatus = new List<StatusBase>();
		private readonly StatusComparer m_comparer = new StatusComparer();
		private Unit m_owner;

		public StatusBase this[int index] => m_currentStatus[index];
		public int Count => m_currentStatus.Count;

		public void Setup(Unit owner)
		{
			m_owner = owner;
		}

		public void Apply(StatusBase statusBase)
		{
			if (Contains(statusBase))
			{
				Stack(statusBase);
			}
			else
			{
				Add(statusBase);
			}

			if (statusBase.StatusData.BuffType == BuffType.Debuff)
			{
				EventLog.Add(new Debuffed(m_owner.name));
			}
		}

		private void Add(StatusBase statusBase)
		{
			m_currentStatus.Add(statusBase);
			statusBase.Activate();
			Added?.Invoke(statusBase);
			Logger.Log("Added", statusBase);
		}

		private void Stack(StatusBase statusBase)
		{
			var targetStatus = m_currentStatus.Find
				(
				 x => m_comparer.Equals(x, statusBase)
				);

			targetStatus.AddStacks(statusBase.Stacks);
			Logger.Log("Stacked", targetStatus);
		}

		public bool Contains(StatusBase statusBase)
		{
			return m_currentStatus.Contains(statusBase, m_comparer);
		}

		public bool Contains(Type type, out StatusBase statusBase)
		{
			var retVal = m_currentStatus.Find(x => x.GetType() == type);
			statusBase = retVal;
			return retVal != null;
		}

		public void UpdateDuration()
		{
			for (var i = m_currentStatus.Count - 1; i >= 0; i--)
			{
				var status = m_currentStatus[i];
				status.Update();
				Logger.Log("Updated", status);

				if (status.IsFinished)
				{
					Remove(status);
				}
			}
		}

		public void Remove(StatusBase statusBase)
		{
			statusBase.Deactivate();
			m_currentStatus.Remove(statusBase);
			Removed?.Invoke(statusBase);
			Logger.Log("Removed", statusBase);
		}
	}
}