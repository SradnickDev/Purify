using System.Collections.Generic;
using Cards.General;
using MessagePack;
using Misc.Events;
using Status.General;
using Units.General;
using UnityEngine;

namespace Status.Types
{
	[MessagePackObject(true)]
	public class DefensiveStanceData : StatusData
	{
		public int Amount;

		public DefensiveStanceData()
		{
			Name = "Defensive Stance";
		}

		public override StatusBase Initialize(Unit owner)
		{
			return new DefensiveStance(this, owner);
		}
	}

	public class DefensiveStance : TriggeredStatus
	{
		private TriggeredAction m_endTrigger;
		public DefensiveStance(StatusData statusData, Unit unit) : base(statusData, unit) { }

		public override bool IsFinished => false;

		public override void SetupTrigger()
		{
			m_endTrigger = new TriggeredAction
			{
				Requirement = new TurnEnd(AffectedUnit.name),
				OnTriggered = RemoveOnPlayerTurnEnd
			};
			EventLog.Register(m_endTrigger);
			StatusData.Trigger.Requirement = new PlayedCardType(CardType.Attack);
		}

		public override void OnTriggerRaised()
		{
			AffectedUnit.ChangeBlock(((DefensiveStanceData) StatusData).Amount * Instances);
		}

		private void RemoveOnPlayerTurnEnd()
		{
			AffectedUnit.StatusContainer.Remove(this);
		}

		public override void Deactivate()
		{
			EventLog.Deregister(m_endTrigger);
			m_endTrigger.Requirement = null;
			m_endTrigger.OnTriggered = null;
			m_endTrigger = null;

			base.Deactivate();
		}
	}
}