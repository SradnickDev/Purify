using UnityEngine;

namespace Battle.General
{
	//Data Paths
	public class Constants
	{
#region Resources

		public const string StarterDeckData = "StarterDeckData";
		public const string SelectionStarterPool = "CardPools/SelectionStarterPool";
		public const string FixedStarterCardPool = "CardPools/FixedStarterPool";
		public const string HiddenCardPool = "CardPools/HiddenPool";
		public const string HiddenCardPoolName = "HiddenPool";
		public const string EncounterCollection = "Encounter/EncounterTemplateCollection";

#endregion

#region SaveData

		public static readonly string PersistentPath = Application.persistentDataPath + "/saves/";
		public const string BattleConfig = "MatchConfig";
		public const string PlayerDeckIdentifier = "PlayerDeck";

#endregion

#region PlayerPrefs

		public static readonly string ChosenPool = "CrusaderPool"; //PlayerPrefs.GetString("ChoosenPool");

#endregion
	}
}