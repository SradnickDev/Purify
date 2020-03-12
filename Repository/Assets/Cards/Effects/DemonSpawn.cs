using Battle.Zones;
using Cards.Effects.General;
using MessagePack;
using Units.Enemy.General;
using Units.General;
using UnityEngine;

namespace Cards.Effects
{
	[MessagePackObject(true)]
	public class DemonSpawn : Effect
	{
		public ScriptableObject EnemiesToSpawn;

		private EnemyZone EnemyZone
		{
			get
			{
				if (m_enemyZone == null)
				{
					m_enemyZone = Object.FindObjectOfType<EnemyZone>();
				}

				return m_enemyZone;
			}
		}

		private EnemyZone m_enemyZone;

		public int MaxSpawnAmount;
		public Effect FallbackEffect;
		private int CurrentSpawnAmount => EnemyZone.MaxEnemyAmount - EnemyZone.CurrentEnemyAmount;
		private bool CanSpawn => CurrentSpawnAmount > 0;
		private int m_spawnAmount;

		public override void Execute(Unit target, Unit from)
		{
			if (CanSpawn)
			{
				var data = (EncounterCollection) EnemiesToSpawn;
				
				m_spawnAmount = CalculateSpawnAmount();
				
				for (var i = 0; i < m_spawnAmount; i++)
				{
					var idx = Random.Range(0, data.EncounterData.Count);
					var enemy = data.EncounterData[idx].Enemies[0];
					EnemyZone.AddToEncounter(enemy);
				}
			}
			else
			{
				FallbackEffect.Execute(target, from);
			}
		}

		private int CalculateSpawnAmount()
		{
			if (CurrentSpawnAmount >= MaxSpawnAmount)
			{
				return MaxSpawnAmount;
			}
			
			return CurrentSpawnAmount - MaxSpawnAmount;
			
		}

		public override object Value(Unit @from, Unit target)
		{
			return CanSpawn ? CalculateSpawnAmount() : FallbackEffect.Value(@from, target);
		}
	}
}