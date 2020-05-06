using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Misc
{
	public class ScenePreload : MonoBehaviour
	{
		[SerializeField] private int m_sceneIndexToPreload = 3;
		private AsyncOperation m_asyncOperation;

		private void Start()
		{
			Preload();
		}

		private void Preload()
		{
			//load scene into memory to reduce performance spikes
			Application.backgroundLoadingPriority = ThreadPriority.BelowNormal;
			m_asyncOperation =
				SceneManager.LoadSceneAsync(m_sceneIndexToPreload, LoadSceneMode.Additive);

			//don't allow to activate the scene or all Awake/Start/OnEnable will be called
			m_asyncOperation.allowSceneActivation = false;
			m_asyncOperation.completed += OnPreloadCompleted;
		}

		private void OnPreloadCompleted(AsyncOperation operation)
		{
			//Unload scene, unity keeps stuff in memeory
			var targetScene = SceneManager.GetSceneByBuildIndex(m_sceneIndexToPreload);
			foreach (var gbj in targetScene.GetRootGameObjects())
			{
				gbj.SetActive(false);
			}

			SceneManager.UnloadSceneAsync(targetScene);
			DestroySceneContext();
		}

		private void DestroySceneContext()
		{
			//is marked as DontDestroyOnLoad 
			//ProjectContext is, even before awake is called, Initialized.
			//Destroy to avoid possible bugs
			var projectContext = FindObjectOfType<ProjectContext>();
			if (projectContext != null)
			{
				Destroy(projectContext.gameObject);
			}
		}
	}
}