using System.Collections.Generic;
using Status.Types;
using Units.General;
using UnityEngine;

namespace Status.General
{
	[RequireComponent(typeof(Unit))]
	public class DisplayStatusContainer : MonoBehaviour
	{
		private Unit m_owner;
		private StatusContainer m_statusContainer;
		private List<StatusData> m_displayStatus = new List<StatusData>();

		private void Start()
		{
			m_owner = GetComponent<Unit>();
			m_statusContainer = m_owner.StatusContainer;
			var dataList = StatusData.LoadDataList();
			m_displayStatus.Add(dataList.Find(x => x.GetType() == typeof(PurityData)));
			m_displayStatus.Add(dataList.Find(x => x.GetType() == typeof(MightData)));
			m_displayStatus.Add(dataList.Find(x => x.GetType() == typeof(CorruptionData)));
			m_displayStatus.Add(dataList.Find(x => x.GetType() == typeof(PerseveranceData)));
		}

		private void Update()
		{
			UpdateDisplayStatusType();
		}

		public void UpdateDisplayStatusType()
		{
			if (m_owner == null) return;

			var hasCorruption =
				m_statusContainer.Contains(typeof(CorruptionDisplayStatus), out var corruption);

			if (m_owner.Soul.CorruptionStacks(m_owner.SoulStackThreshold) > 0 && !hasCorruption)
			{
				var data = m_displayStatus.Find(x => x.GetType() == typeof(CorruptionData));
				m_statusContainer.Apply(data.Initialize(m_owner));
			}
			else if (hasCorruption && m_owner.Soul.CorruptionStacks(m_owner.SoulStackThreshold) == 0)
			{
				m_statusContainer.Remove(corruption);
			}

			var hasPurity = m_statusContainer.Contains(typeof(PurityDisplayStatus), out var purity);
			if (m_owner.Soul.PurityStacks(m_owner.SoulStackThreshold) > 0 && !hasPurity)
			{
				var data = m_displayStatus.Find(x => x.GetType() == typeof(PurityData));
				m_statusContainer.Apply(data.Initialize(m_owner));
			}
			else if (hasPurity && m_owner.Soul.PurityStacks(m_owner.SoulStackThreshold) == 0)
			{
				m_statusContainer.Remove(purity);
			}

			var hasMight = m_statusContainer.Contains(typeof(MightDisplayStatus), out var might);
			if (m_owner.Might.Current != 0 && !hasMight)
			{
				var data = m_displayStatus.Find(x => x.GetType() == typeof(MightData));
				m_statusContainer.Apply(data.Initialize(m_owner));
			}
			else if (m_owner.Might.Current == 0 && hasMight)
			{
				m_statusContainer.Remove(might);
			}

			var hasPerseverance =
				m_statusContainer.Contains(typeof(PerseveranceDisplayStatus), out var perseverance);
			if (m_owner.Perseverance.Current != 0 && !hasPerseverance)
			{
				var data = m_displayStatus.Find(x => x.GetType() == typeof(PerseveranceData));
				m_statusContainer.Apply(data.Initialize(m_owner));
			}
			else if (m_owner.Perseverance.Current == 0 && hasPerseverance)
			{
				m_statusContainer.Remove(perseverance);
			}
		}
	}
}