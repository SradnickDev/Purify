using Battle.General;
using OptionMenu;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 0649
namespace Resting
{
	[RequireComponent(typeof(Button))]
	public class RestButton : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI m_counterDisplay;

		private Button m_button;

		private void Start()
		{
			Setup();
		}

		private void Setup()
		{
			m_button = GetComponent<Button>();
			var config = Options.LoadConfigData();
			ConfigureButton(config);
		}

		private void ConfigureButton(BattleConfig config)
		{
			m_counterDisplay.text = config.RestCount.ToString();
			m_button.onClick.AddListener(() =>
			{
				config.RestCount--;
				Options.SaveConfigData(config);
			});
			m_button.interactable = config.RestCount > 0;
		}
	}
}
#pragma warning restore 0649
