using System;
using Battle.General;
using Misc.PopUp;
using OptionMenu;
using TMPro;
using UnityEngine;

#pragma warning disable 0649
namespace Resting
{
	public class RestMenu : MonoBehaviour
	{
		[SerializeField] private int m_defaultRestPoints = 12;
		[SerializeField] private TextMeshProUGUI m_textMesh;
		public BattleConfig BattleConfig { get; private set; }
		public int RestPoints { get; set; }

		private string RestPointsCounter
		{
			set
			{
				if (m_textMesh)
				{
					m_textMesh.text = value;
				}
			}
		}

		private void OnEnable()
		{
			PopUpHandler.Instance.CloseAll();
		}

		private void Start()
		{
			LoadConfig();
			SetDefaultRestPoints();
		}

		private void LoadConfig() => BattleConfig = Options.LoadConfigData();

		private void SetDefaultRestPoints()
		{
			RestPoints = m_defaultRestPoints;
			RestPointsCounter = RestPoints.ToString();
		}

		public bool HasRestPoints(int value) => RestPoints >= value;

		public void UseRestPoints(int amount)
		{
			RestPoints -= amount;
			RestPointsCounter = RestPoints.ToString();
		}

		public void Save() => Options.SaveConfigData(BattleConfig);
	}
}
#pragma warning restore 0649