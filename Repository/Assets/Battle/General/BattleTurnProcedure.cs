using System;
using System.Collections;
using Battle.GameStates;
using Misc;
using Misc.Events;
using OptionMenu;
using Status;
using Units.Enemy.General;
using Units.Player.General;
using UnityEngine;
using Utilities;
using Zenject;

namespace Battle.General
{
	public class BattleTurnProcedure : MonoBehaviour
	{
		public Action PlayerWon;
		public Action PlayerLost;
		public Action PlayerTurnStart;
		public Action OpponentTurnStart;

		public BattleConfig Config { get; set; }

		public InitializeState Initialize;
		public PlayerTurnState PlayerTurn;
		public OpponentTurnState OpponentTurn;

		[SerializeField] private float m_timeAfterTurnCycle = 1f;

		[Inject]
		private Player m_player = null;

		[Inject]
		private Encounter m_encounter = null;

		public void Start()
		{
			Config = LoadConfig();

			BattleInfo.Player = m_player;
			BattleInfo.Encounter = m_encounter;
			BattleInfo.CurrentGameState = null;

			Initialize = new InitializeState(Config);
			PlayerTurn = new PlayerTurnState();
			OpponentTurn = new OpponentTurnState();

			StartTurnProcedure();
		}

		private void StartTurnProcedure()
		{
			StartCoroutine(TurnProcedure());
		}

		public static BattleConfig LoadConfig() =>
			Options.LoadConfigData();

		private IEnumerator TurnProcedure()
		{
			//cached to reduce produced garbage
			var waitForSeconds = new WaitForSeconds(m_timeAfterTurnCycle);

			var waitPlayerPhaseCompleted =
				new WaitUntil(()
								  =>
							  {
								  ChangeState(PlayerTurn);

								  return PlayerTurn.Execute() || MatchEnd();
							  });


			var waitForOpponentPhaseCompleted =
				new WaitUntil(()
								  =>
							  {
								  ChangeState(OpponentTurn);

								  return OpponentTurn.Execute() || MatchEnd();
							  });

			Initialize.Execute();
			ChangeState(Initialize);

			while (!MatchEnd())
			{
				yield return waitPlayerPhaseCompleted;
				yield return waitForOpponentPhaseCompleted;

				yield return waitForSeconds;
			}

			EmitMatchResult();
			yield return null;
		}

		public void EndPlayerTurn()
		{
			if (BattleInfo.CurrentGameState == PlayerTurn)
			{
				PlayerTurn.EndState();
			}
		}

		private void ChangeState(GameState state)
		{
			if (BattleInfo.CurrentGameState != state)
			{
				BattleInfo.CurrentGameState = state;

				if (BattleInfo.CurrentGameState is PlayerTurnState)
				{
					if (!BattleInfo.Player.IsDead())
					{
						PlayerTurnStart?.Invoke();
					}
				}

				if (BattleInfo.CurrentGameState is OpponentTurnState)
				{
					if (!BattleInfo.Encounter.OpponentsDied())
					{
						OpponentTurnStart?.Invoke();
					}
				}
			}
		}

		private void EmitMatchResult()
		{
			CardTurnLog.Clear();

			if (BattleInfo.Player.IsDead())
			{
				OnPlayerLost();
			}

			if (BattleInfo.Encounter.OpponentsDied())
			{
				OnPlayerWon();
			}
		}

		private void OnPlayerLost()
		{
			PlayerLost?.Invoke();
			EventLog.Add(new PlayerLost());
			EventLog.Clear();
		}

		private void OnPlayerWon()
		{
			PlayerWon?.Invoke();
			EventLog.Add(new PlayerWon());
			EventLog.Clear();
			Save();
		}

		private void Save()
		{
			Config.BattleCount++;
			Config.Health = new Range(m_player.Health.Current, m_player.Health.Max);
			Config.Soul = m_player.Soul.Current;
			Options.SaveConfigData(Config);
		}

		public bool MatchEnd() => BattleInfo.Player.IsDead() ||
								  BattleInfo.Encounter.OpponentsDied();
	}
}