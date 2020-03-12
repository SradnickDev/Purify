using System.Collections.Generic;
using Battle.General;
using Cards.General;
using Units.General;
using UnityEngine;

namespace Units.Enemy.General
{
	[RequireComponent(typeof(Enemy))]
	public class Intention : MonoBehaviour
	{
		[SerializeField] private Enemy m_enemy = null;
		[SerializeField] private IntentionView m_intentionView = null;

		private void Update()
		{
			UpdateIntention(m_enemy.NextAttack, m_enemy);
		}

		public void UpdateIntention(Attack attack, Unit unit)
		{
			m_intentionView.Icon = attack.Icon;
			m_intentionView.AmountInfo = attack.Value(unit, BattleInfo.Player).ToString();
			m_intentionView.Header = attack.Name;

			ParseDescription(attack, unit);
		}

		private void ParseDescription(Attack attack, Unit unit)
		{
			var description = attack.Description;

			description = DescriptionParser.Parse
				(
				 description,
				 unit,
				 BattleInfo.Player,
				 new List<IDescriptionValue>(attack.Effect)
				);

			m_intentionView.Description = description;
		}
	}
}