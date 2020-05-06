using Battle.General;
using OptionMenu;
using UnityEngine;
using Utilities;
using Object = UnityEngine.Object;

namespace Units.Enemy.General
{
	public static class EncounterFactory
	{
		/// <summary>
		/// Load Encounter based on config.
		/// </summary>
		public static Encounter Build(BattleConfig config)
		{
			var encounterData = Load(config);
			var enemies = Create(encounterData);
			return enemies;
		}

		/// <summary>
		/// Create a new Enemy.
		/// </summary>
		/// <param name="enemy"></param>
		/// <returns></returns>
		public static Enemy Build(Enemy enemy)
		{
			var newEnemy = Object.Instantiate(enemy);
			newEnemy.Pattern = LoadAttackPattern(enemy);
			newEnemy.name += RNG.GetNewSeed();
			return newEnemy;
		}

		private static EncounterData Load(BattleConfig config)
		{
			var encounterCollection =
				(EncounterCollection) Resources.Load(Constants.EncounterCollection);

			var encounterIdx = Mathf.Clamp(config.BattleCount, 0,
										   encounterCollection.EncounterData.Count - 1);
			return encounterCollection.EncounterData[encounterIdx];
		}

		private static Encounter Create(EncounterData data)
		{
			var retVal = new Encounter();
			foreach (var enemy in data.Enemies)
			{
				var newEnemy = Build(enemy);
				retVal.Enemies.Add(newEnemy);
			}

			return retVal;
		}

		/// <summary>
		/// Load data from a json file, convert it to a Attack pattern.
		/// </summary>
		/// <param name="prefab"></param>
		/// <returns></returns>
		private static AttackPattern LoadAttackPattern(Object prefab)
		{
			var data = Resources.Load<TextAsset>("AttackPattern/" + prefab.name);
			return PersistentJson.Create<AttackPattern>(data.text);
		}
	}
}