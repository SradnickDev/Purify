using OptionMenu;
using SCT;
using Units.Enemy.General;
using UnityEngine;
using UnityEngine.Events;

#pragma warning disable 0649
namespace Battle.General
{
	/// <summary>
	/// Provides drag n drop Events.
	/// </summary>
	[RequireComponent(typeof(BattleTurnProcedure))]
	public class BattleEventView : MonoBehaviour
	{
		[SerializeField] private EncounterCollection Collection;

		public UnityEvent OnPlayerWon;
		public UnityEvent OnPlayerLost;
		public UnityEvent OnGameEnd;
		public UnityEvent OnPlayerTurn;
		public UnityEvent OnEnemyTurn;

		private BattleTurnProcedure m_turnProcedure;

		private void Start()
		{
			m_turnProcedure = GetComponent<BattleTurnProcedure>();
			AddListener();
		}

		private void AddListener()
		{
			m_turnProcedure.PlayerWon += () =>
			{
				if (IsValidGameEnd())
				{
					Options.ResetConfigData();
					OnGameEnd?.Invoke();
				}
				else
				{
					OnPlayerWon?.Invoke();
				}
			};

			m_turnProcedure.PlayerLost += () => OnPlayerLost?.Invoke();
			m_turnProcedure.PlayerTurnStart += () =>
			{
				ScriptableTextDisplay.Instance.InitializeScriptableText(2, Vector3.zero, "Player Turn!");
				OnPlayerTurn?.Invoke();
			};
			m_turnProcedure.OpponentTurnStart += () =>
			{
				ScriptableTextDisplay.Instance.InitializeScriptableText(2, Vector3.zero, "Enemy Turn!");
				OnEnemyTurn?.Invoke();
			};
		}

		public bool IsValidGameEnd()
		{
			var config = Options.LoadConfigData();
			var maxRounds = Collection.EncounterData.Count - 1;
			return config.BattleCount >= maxRounds;
		}
	}
}
#pragma warning restore 0649