using System.Collections;
using System.Collections.Generic;
using Battle.General;
using Misc.Events;
using Status;
using Units.Enemy.General;
using UnityEngine;

namespace Battle.GameStates
{
	[System.Serializable]
	public class OpponentTurnState : GameState
	{
		[SerializeField] private float m_timeBetweenEnemies = 0.5f;
		private bool m_attackPhase = false;
		private WaitForSeconds m_waitForSeconds;

		protected override void Start()
		{
			//cache to reduce garbage
			m_waitForSeconds = new WaitForSeconds(m_timeBetweenEnemies);
			StatusPhase();
		}

		/// <summary>
		/// Buffs and Debuff's get updated.
		/// </summary>
		private void StatusPhase()
		{
			var encounter = BattleInfo.Encounter;

			encounter.Foreach(enemy =>
			{
				enemy.Defense.Current = 0;
				EventLog.Add(new TurnStart(enemy.name));
				enemy.StatusContainer.UpdateDuration();
			});

			m_attackPhase = true;
		}

		protected override void Update()
		{
			if (m_attackPhase)
			{
				//using player to Execute a coroutine
				//coroutines are a Monobehaviour thing
				BattleInfo.Player.StartCoroutine(AttackPhase());
				m_attackPhase = false;
			}
		}

		/// <summary>
		/// Enemies act out his moves.
		/// </summary>
		IEnumerator AttackPhase()
		{
			yield return m_waitForSeconds;

			//cache encounter, new enemies that will be added do not attack this turn
			var encounter = new List<Enemy>(BattleInfo.Encounter);

			foreach (var enemy in encounter)
			{
				var timeForEffects = m_timeBetweenEnemies / enemy.NextAttack.Effect.Count;
				enemy.StartCoroutine(enemy.Attack(new WaitForSeconds(timeForEffects)));
				yield return m_waitForSeconds;
			}

			IsCompleted = true;
		}

		protected override void Finish()
		{
			EndPhase();
		}

		private void EndPhase()
		{
			var encounter = BattleInfo.Encounter;

			encounter.Foreach(x =>
			{
				if (x)
				{
					EventLog.Add(new TurnEnd(x.name));
				}
			});
		}
	}
}