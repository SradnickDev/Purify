using OptionMenu;
using Units.Enemy.General;
using UnityEngine;

#pragma warning disable 0649
namespace Battle.Background
{
	public class Background : MonoBehaviour
	{
		[SerializeField] private GameObject m_defaultBackground;
		[SerializeField] private GameObject m_bossBackground;
		[SerializeField] private EncounterCollection m_encounterCollection;

		private void Awake()
		{
			ManageBackgrounds();
		}

		/// <summary>
		/// Based  on battle count default or boss background will be used.
		/// </summary>
		private void ManageBackgrounds()
		{
			var battleCount = Options.LoadConfigData().BattleCount;
			if (battleCount >= m_encounterCollection.EncounterData.Count-1)
			{
				UseBossBackground();
			}
			else
			{
				UseDefaultBackground();
			}
		}

		private void UseDefaultBackground()
		{
			if (m_defaultBackground != null)
			{
				m_defaultBackground.SetActive(true);
			}

			if (m_bossBackground != null)
			{
				m_bossBackground.SetActive(false);
			}
		}

		private void UseBossBackground()
		{
			if (m_defaultBackground != null)
			{
				m_defaultBackground.SetActive(false);
			}

			if (m_bossBackground != null)
			{
				m_bossBackground.SetActive(true);
			}
		}
	}
}
#pragma warning restore 0649