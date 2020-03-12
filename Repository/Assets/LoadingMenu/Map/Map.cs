using System.Collections.Generic;
using OptionMenu;
using UnityEngine;

#pragma warning disable 0649
namespace LoadingMenu.Map
{
	public class Map : MonoBehaviour
	{
		[SerializeField] private List<MapPathPoint> m_points;
		private int m_battleCount;

		private void Start()
		{
			Setup();
		}

		public void Setup()
		{
			m_battleCount = Options.LoadConfigData().BattleCount;

			for (var i = 0; i < m_points.Count; i++)
			{
				var point = m_points[i];
				point.Setup(i < m_battleCount);
			}
			var idx = Mathf.Clamp(m_battleCount, 0, m_points.Count - 1);
			m_points[idx].Setup(true);
			m_points[idx].PlayAnimation();
		}
	}
}
#pragma warning restore 0649