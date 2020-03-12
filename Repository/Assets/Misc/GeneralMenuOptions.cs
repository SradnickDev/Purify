using LoadingMenu;
using OptionMenu;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Misc
{
	public class GeneralMenuOptions : MonoBehaviour
	{
		public void LoadMainMenu()
		{
			LoadingScreen.LoadScene(1);
		}

		public void ReloadSceneWithMap()
		{
			var scene = SceneManager.GetActiveScene();
			LoadingScreen.LoadSceneWithMap(scene.buildIndex);
		}

		public void ReloadSceneWithTutorial()
		{
			var scene = SceneManager.GetActiveScene();
			LoadingScreen.LoadSceneWithTutorial(scene.buildIndex);
		}

		public void ReloadScene()
		{
			var scene = SceneManager.GetActiveScene();
			SceneManager.LoadScene(scene.name);
		}

		public void DeleteRun()
		{
			Options.ResetConfigData();
		}

		public void Quit()
		{
			Application.Quit();
		}
	}
}