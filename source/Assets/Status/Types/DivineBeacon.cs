using System.Collections.Generic;
using Battle.General;
using Cards.General;
using MessagePack;
using Misc.Events;
using Status.General;
using Units.General;
using UnityEngine;

namespace Status.Types
{
	[MessagePackObject(true)]
	public class DivineBeaconData : StatusData
	{
		public int Percentage;

		public DivineBeaconData()
		{
			Name = "Divine Beacon";
		}

		public override StatusBase Initialize(Unit owner)
		{
			return new DivineBeacon(this, owner);
		}
	}

	public class DivineBeacon : TriggeredStatus
	{
		private TriggeredAction m_attackTrigger;
		private bool m_attacked = false;
		public DivineBeacon(StatusData statusData, Unit unit) : base(statusData, unit) { }

		public override bool IsFinished => false;

		public override void SetupTrigger()
		{
			m_attackTrigger = new TriggeredAction
			{
				Requirement = new PlayedCardType(CardType.Attack),
				OnTriggered = () => { m_attacked = true; }
			};

			EventLog.Register(m_attackTrigger);

			StatusData.Trigger.Requirement = new TurnEnd() {Key = AffectedUnit.name};
		}

		public override void OnTriggerRaised()
		{
			if (!m_attacked)
			{
				TriggerPurifyEffect();
			}

			m_attacked = false;
		}

		private void TriggerPurifyEffect()
		{
			BattleInfo.SolveTargetSelection(null, TargetType.AllEnemies, AffectedUnit, PurifyEnemy);
		}

		private void PurifyEnemy(Unit target, Unit player)
		{
			var percentage = ((DivineBeaconData) StatusData).Percentage / 100f;
			percentage *= Instances;
			target.Soul.Current += Mathf.FloorToInt(player.Defense.Current * percentage);
		}

		public override void Deactivate()
		{
			EventLog.Deregister(m_attackTrigger);
			m_attackTrigger.OnTriggered = null;
			m_attackTrigger = null;
			base.Deactivate();
		}
	}
}