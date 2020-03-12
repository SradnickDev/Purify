using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Utilities
{
	/// <summary>
	/// Replaces Unity's Pointer callbacks to get something simpler.
	/// </summary>
	[RequireComponent(typeof(GraphicRaycaster))]
	public class SimplePointer : MonoBehaviour
	{
		private GraphicRaycaster m_raycaster;
		private PointerEventData m_pointerEventData;
		private GameObject m_pointerTarget;
		private List<RaycastResult> m_raycastResults = new List<RaycastResult>();

		private void Start()
		{
			m_raycaster = GetComponent<GraphicRaycaster>();
			m_pointerEventData = new PointerEventData(EventSystem.current);
		}

		public void Update() => PointerTargetUpdate();

		/// <summary>
		/// Checks if something new is under the Mouse.
		/// </summary>
		private void PointerTargetUpdate()
		{
			Raycast();

			if (m_raycastResults.Count <= 0)
			{
				TriggerExitCallback();

				m_pointerTarget = null;
				return;
			}

			NewTarget();
		}

		/// <summary>
		/// Trigger's callback for new Target.
		/// </summary>
		private void NewTarget()
		{
			if (m_raycastResults[0].gameObject != m_pointerTarget)
			{
				var newSelection = m_raycastResults[0].gameObject;

				TriggerExitCallback();

				TriggerEnterCallback(newSelection);

				m_pointerTarget = newSelection;
			}
		}

		/// <summary>
		
		/// Event data used to raycast for fitting objects.
		/// </summary>
		private void Raycast()
		{
			m_raycastResults = new List<RaycastResult>();
			m_pointerEventData.position = Input.mousePosition;
			m_raycaster.Raycast(m_pointerEventData, m_raycastResults);
		}

		private static void TriggerEnterCallback(GameObject newSelection)
		{
			foreach (var pointer in newSelection.HasComponents<ISimplePointer>())
			{
				if (pointer.IsActive)
				{
					pointer?.OnEnter();
				}
			}
		}

		private void TriggerExitCallback()
		{
			if (m_pointerTarget)
			{
				foreach (var pointer in m_pointerTarget.HasComponents<ISimplePointer>())
				{
					if (pointer.IsActive)
					{
						pointer?.OnExit();
					}
				}
			}
		}
	}
}