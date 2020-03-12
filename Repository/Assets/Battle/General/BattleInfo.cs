using System;
using System.Collections.Generic;
using Battle.GameStates;
using Cards.General;
using Units.Enemy.General;
using Units.General;
using Units.Player.General;

namespace Battle.General
{
	public static class BattleInfo
	{
		public static Player Player;
		public static Encounter Encounter;
		public static GameState CurrentGameState;
		public static TurnPhase TurnPhase;

		/// <summary>
		/// Based on given information and current game state, emits action on targets.
		/// </summary>
		/// <param name="selectedTarget">Possible target</param>
		/// <param name="targetType">What group of targets</param>
		/// <param name="from">Origin.</param>
		/// <param name="action">Will be executed on targets</param>
		public static void SolveTargetSelection(Unit selectedTarget
												, TargetType targetType
												, Unit from
												, Action<Unit, Unit> action)
		{
			var targets = Target(targetType, selectedTarget);

			foreach (var target in targets)
			{
				action(target, from);
			}
		}

		private static IEnumerable<Unit> Target(TargetType targetType, Unit target)
		{
			switch (targetType)
			{
				case TargetType.Self:
					switch (CurrentGameState)
					{
						case OpponentTurnState _:
							yield return Encounter.CurrentActive;
							break;
						case PlayerTurnState _:
							yield return Player;
							break;
					}

					break;
				case TargetType.Single:
					switch (CurrentGameState)
					{
						case OpponentTurnState _:
							yield return Player;
							break;
						case PlayerTurnState _:
							yield return target;
							break;
					}

					break;
				case TargetType.AllEnemies:

					foreach (var opponent in Encounter)
					{
						yield return opponent;
					}

					break;
				case TargetType.AllUnits:
					foreach (var opponent in Encounter)
					{
						yield return opponent;
					}

					yield return Player;
					break;
			}
		}
	}
}