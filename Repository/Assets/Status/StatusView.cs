using System.Collections.Generic;
using Status.General;
using Status.Types;
using Units.General;
using UnityEngine;
using Utilities;

#pragma warning disable 0649

namespace Status
{
	public class StatusView : MonoBehaviour
	{
		[SerializeField] private StatusIcon m_statusIcon;
		[SerializeField] private Unit m_target;
		[SerializeField] private Transform m_parentTarget;
		[SerializeField] private Color m_debuffColor = Color.magenta;
		[SerializeField] private Color m_buffColor = Color.yellow;

		private StatusContainer m_statusContainer;

		private List<KeyValuePair<StatusBase, StatusIcon>> m_icons =
			new List<KeyValuePair<StatusBase, StatusIcon>>();

		private void Start()
		{
			RetrieveContainer();
		}

		private void RetrieveContainer()
		{
			m_statusContainer = m_target.StatusContainer;
			RegisterEvents();
		}

		private void RegisterEvents()
		{
			m_statusContainer.Added += OnStatusAdded;
			m_statusContainer.Removed += OnStatusRemoved;
		}

		/// <summary>
		/// Instantiate a new Status. Set the visual part and parse the Header.
		/// </summary>
		/// <param name="st"></param>
		private void OnStatusAdded(StatusBase st)
		{
			var newIcon = Instantiate(m_statusIcon, m_parentTarget);

			newIcon.Set(st.StatusData.Icon, st.ToString(), st.StatusData.AudioClip);
			newIcon.Name =
				GeneralExtensions.ParseStatusHeader(st.StatusData.Name, st.StatusData.BuffType,
													m_buffColor, m_debuffColor);

			newIcon.Description = st.StatusData.Description;

			m_icons.Add(new KeyValuePair<StatusBase, StatusIcon>(st, newIcon));
		}

		private void Update() => StatusUpdate();

		/// <summary>
		/// Iterate through all status and Updates them.
		/// </summary>
		private void StatusUpdate()
		{
			foreach (var keyValuePair in m_icons)
			{
				var status = keyValuePair.Key;
				var icon = keyValuePair.Value;
				icon.UpdateDisplay(status.ToString());
			}
		}

		/// <summary>
		/// Find  status, remove and destroy it.
		/// </summary>
		/// <param name="st"></param>
		private void OnStatusRemoved(StatusBase st)
		{
			var targetPair = m_icons.Find(x => x.Key == st);
			Destroy(targetPair.Value.gameObject);
			m_icons.Remove(targetPair);
		}

		private void OnDestroy() => RemoveEvents();

		private void RemoveEvents()
		{
			if (m_statusContainer != null)
			{
				m_statusContainer.Added -= OnStatusAdded;
				m_statusContainer.Removed -= OnStatusRemoved;
			}
		}
	}
}
#pragma warning restore 0649