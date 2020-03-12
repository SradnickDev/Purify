using System.Collections.Generic;
using System.Linq;
using Cards.General;
using Units.Enemy.General;
using UnityEngine;
using Utilities;
using Zenject;

#pragma warning disable 0649
namespace Battle.Zones
{
	public class EnemyZone : MonoBehaviour
	{
		public const int MaxEnemyAmount = 3;

		[SerializeField, Range(0, 2f)] private float m_usedScreenWidth;
		[SerializeField, Range(0f, 2f)] private float m_usedAnchorHeight;
		public int CurrentEnemyAmount => transform.childCount;
		private Encounter m_currentEncounter;
		private Enemy[] m_enemies = new Enemy[MaxEnemyAmount];

		[Inject]
		private void SetEncounter(Encounter encounter)
		{
			m_currentEncounter = encounter;
			encounter.Changed += () => { SetEnemies(encounter); };
		}

		private void OnEnable()
		{
			RootCanvas.ResolutionChanged += UpdateEnemyPosition;
		}

		private void OnDisable()
		{
			RootCanvas.ResolutionChanged -= UpdateEnemyPosition;
		}

		/// <summary>
		/// Add new Enemy to the current Encounter. Will be ignored this turn
		/// </summary>
		/// <param name="enemy"></param>
		public void AddToEncounter(Enemy enemy)
		{
			if (m_currentEncounter.Count < MaxEnemyAmount)
			{
				var newEnemy = EncounterFactory.Build(enemy);
				m_currentEncounter.Add(newEnemy);
			}
		}

		/// <summary>
		/// Update Enemy Position.
		/// </summary>
		/// <param name="encounter"></param>
		private void SetEnemies(Encounter encounter)
		{
			foreach (var enemy in encounter)
			{
				enemy.transform.SetParent(this.transform, false);
			}

			SortEnemies(encounter);

			DoRadialLayout(m_enemies);
		}

		/// <summary>
		/// Based on specific ZonePosition, sort the enemies.
		/// </summary>
		/// <param name="encounter">Current Encounter</param>
		private void SortEnemies(Encounter encounter)
		{
			m_enemies = encounter.ToArray();

			Enemy middle = encounter.ToList().Find(x => x.ZonePosition == ZonePosition.Middle);

			if (middle != null)
			{
				var temp = m_enemies;
				m_enemies = new Enemy[MaxEnemyAmount];

				for (var i = 0; i < temp.Length; i++)
				{
					m_enemies[i] = temp[i];
				}

				var middleIdx = m_enemies.ToList().FindIndex(x => x == middle);
				var currentMid = m_enemies[1];

				//hard coded center...
				m_enemies[1] = middle;
				m_enemies[middleIdx] = currentMid;
			}
		}

		private void UpdateEnemyPosition()
		{
			DoRadialLayout(m_enemies);
		}

		/// <summary>
		/// Enemy positioning based on screen width and height.
		/// Using Quadratic Curve.
		/// </summary>
		private void DoRadialLayout(IReadOnlyList<Enemy> enemies)
		{
			var halfWidth = Screen.width / 2f;
			var left = halfWidth - halfWidth * m_usedScreenWidth;
			var right = halfWidth + halfWidth * m_usedScreenWidth;
			var height = Screen.height;

			var leftStart = new Vector2(left, 0);
			var rightEnd = new Vector2(right, 0);
			var anchor = new Vector2(halfWidth, height * m_usedAnchorHeight);


			for (var i = 0; i < enemies.Count; i++)
			{
				var current = enemies[i];
				if (current == null) continue;
				var percentage = (i + 1) / (float) (enemies.Count + 1);

				var pos = GeneralExtensions.CalculateQuadraticCurve(percentage,
																	leftStart,
																	anchor,
																	rightEnd);

				current.transform.position = pos;
			}
		}
	}
}
#pragma warning restore 0649