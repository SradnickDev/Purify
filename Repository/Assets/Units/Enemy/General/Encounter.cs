using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Units.Enemy.General
{
	/// <summary>
	/// Custom Collection type that contains Enemies.
	/// </summary>
	public class Encounter : IEnumerable<Enemy>
	{
		public event Action Changed;

		private List<Enemy> m_enemies = new List<Enemy>();
		public Enemy CurrentActive => m_currentActive;
		private Enemy m_currentActive;

		public List<Enemy> Enemies
		{
			get => m_enemies;
			set
			{
				m_enemies = value;
				Changed?.Invoke();
			}
		}

		public void Add(Enemy enemy)
		{
			Enemies.Add(enemy);
			Changed?.Invoke();
		}

		public int Count => m_enemies.Count;

		public IEnumerator<Enemy> GetEnumerator()
		{
			m_enemies.RemoveAll(x => x == null);
			foreach (var enemy in Enemies)
			{
				m_currentActive = enemy;
				yield return enemy;
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Foreach(Action<Enemy> action)
		{
			foreach (var enemy in m_enemies)
			{
				action?.Invoke(enemy);
			}
		}
	}
}