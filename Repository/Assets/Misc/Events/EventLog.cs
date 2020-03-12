using System.Collections.Generic;
using Status.General;
using Utilities;

namespace Misc.Events
{
	public static class EventLog
	{
		private static CardEvent m_event = null;
		private static List<TriggeredAction> m_actions = new List<TriggeredAction>();
		private static CardEventComparer m_comparer = new CardEventComparer();

		/// <summary>
		/// Add a new event.Trigger action that match with this event.
		/// </summary>
		public static void Add(CardEvent evt)
		{
			m_event = evt;
			Logger.Log("EventLog", $"{evt} added.");
			CheckRequirements();
		}

		/// <summary>
		/// Register actions that can be triggered.
		/// </summary>
		public static void Register(params TriggeredAction[] actions)
		{
			foreach (var action in actions)
			{
				m_actions.Add(action);
			}
		}

		/// <summary>
		/// Register action that can be triggered.
		/// </summary>
		public static void Register(TriggeredAction action)
		{
			m_actions.Add(action);
		}

		/// <summary>
		/// Remove a action.
		/// </summary>
		public static void Deregister(TriggeredAction action)
		{
			m_actions.Remove(action);
		}

		/// <summary>
		/// Remove actions.
		/// </summary>
		public static void Deregister(params TriggeredAction[] action)
		{
			foreach (var triggeredAction in action)
			{
				m_actions.Remove(triggeredAction);
			}
		}

		/// <summary>
		/// Check if one Action meets the requirements.
		/// </summary>
		private static void CheckRequirements()
		{
			var saveCopy = new List<TriggeredAction>(m_actions);
			foreach (var action in saveCopy)
			{
				CheckRequirements(action);
			}
		}

		/// <summary>
		/// Check if one Action meets the requirements and invokes the Action.
		/// </summary>
		private static void CheckRequirements(TriggeredAction action)
		{
			var requirements = action.Requirement;
			var evt = m_event;

			if (m_comparer.Equals(requirements, evt))
			{
				action.OnTriggered?.Invoke();
			}
		}

		public static void Clear()
		{
			m_actions = new List<TriggeredAction>();
		}
	}
}