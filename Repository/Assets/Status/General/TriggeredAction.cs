using System;
using Misc.Events;

namespace Status.General
{
	public class TriggeredAction
	{
		public CardEvent Requirement;
		public Action OnTriggered;
		public TriggeredAction() { }

		public TriggeredAction(CardEvent requirement, Action onTriggered)
		{
			Requirement = requirement;
			OnTriggered = onTriggered;
		}
	}
}