using Battle.General;
using Deck;
using Units.Enemy.General;
using Utilities;

namespace Battle.GameStates
{
	/// <summary>
	/// Setup Player DrawPile and Enemies based on config.
	/// </summary>
	public class InitializeState : GameState
	{
		private readonly BattleConfig m_config;
		private readonly DeckSaveData m_deckSaveData;

		public InitializeState(BattleConfig config)
		{
			m_deckSaveData = DeckUtility.LoadSavedDeckData();
			m_config = config;
		}

		protected override void Start()
		{
			SetUpPlayer();
			CreateDeck();
			CreateEnemies();
		}

		/// <summary>
		/// Creating deck from previous loaded data.
		/// </summary>
		private void CreateDeck()
		{
			var deck = DeckFactory.Build(m_deckSaveData, BattleInfo.Player).Cards;
			deck.Shuffle();
			BattleInfo.Player.CardDeck.Cards = deck;
		}

		/// <summary>
		/// Based on battle count that is stored in the config file, creating an Encounter.
		/// </summary>
		private void CreateEnemies()
		{
			BattleInfo.Encounter.Enemies = EncounterFactory.Build(m_config).Enemies;
		}

		/// <summary>
		/// Set previous loaded save data.
		/// </summary>
		private void SetUpPlayer()
		{
			if (m_config.BattleCount == 0) return;

			BattleInfo.Player.Health.Set(m_config.Health.Min,
										 m_config.Health.Max);

			BattleInfo.Player.Soul.Set(BattleInfo.Player.Soul.Min,
									   m_config.Soul,
									   m_config.Health.Max);
		}

#region Ignore

		//can be ignored,not be called
		protected override void Update() { }

		//can be ignored,not be called
		protected override void Finish() { }

#endregion
	}
}