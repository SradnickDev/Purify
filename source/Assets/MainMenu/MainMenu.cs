using LoadingMenu;
using OptionMenu;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using AudioSettings = OptionMenu.AudioSettings;

#pragma warning disable 0649
public class MainMenu : MonoBehaviour
{
	[SerializeField] private Button m_abbondonRun;
	[SerializeField] private AudioSettings m_audioSettings;

	private void Start()
	{
		UpdateButtonState();
	}

	public void StartBattle()
	{
		if (Options.LoadConfigData().BattleCount > 0)
		{
			OnContinue();
		}
		else
		{
			OnStart();
		}
	}

	private void OnStart()
	{
		LoadingScreen.LoadSceneWithTutorial(2);
	}

	private void OnContinue()
	{
		LoadingScreen.LoadSceneWithMap(3);
	}

	public void AbbandonRun()
	{
		Options.ResetConfigData();
		UpdateButtonState();
	}

	private void UpdateButtonState()
	{
		var loadConfigData = Options.LoadConfigData();
		m_abbondonRun.interactable = loadConfigData.BattleCount != 0;
		m_abbondonRun.GetComponentInChildren<TextMeshProUGUI>().color =
			m_abbondonRun.interactable ? Color.white : new Color(1f, 1f, 1f, 0.3f);
	}

	public void QuitApplication()
	{
		Application.Quit();
	}
}
#pragma warning restore 0649