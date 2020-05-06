using System.Collections;
using Animateables;
using LoadingMenu.TutorialScreen;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#pragma warning disable 0649
namespace LoadingMenu
{
	public class LoadingScreen : MonoBehaviour
	{
		[SerializeField] private Map.Map m_map;
		[SerializeField] private Tutorial m_tutorial;

		[Header("Loading Options")]
		[SerializeField] private float m_fadInDuration = 1;

		[SerializeField] private float m_fadeOutDuration = 1;
		[SerializeField] private Image m_overlay;
		[SerializeField] private TextMeshProUGUI m_infoDisplay;
		[SerializeField] private string m_infoText = "click to skip!";
		[SerializeField] private AnimateableScale m_animateableScale;

		private static bool m_isLoading = false;
		private static int m_sceneToLoad = -1;
		private static int m_loadingSceneIndex = 6;
		private static bool m_useMap;
		private static bool m_useTutorial;
		private AsyncOperation m_asyncOperation;

		public static void LoadScene(int sceneIndex)
		{
			if (m_isLoading) return;
			m_useMap = false;
			m_useTutorial = false;

			m_isLoading = true;
			m_sceneToLoad = sceneIndex;
			Application.backgroundLoadingPriority = ThreadPriority.High;
			SceneManager.LoadScene(m_loadingSceneIndex, LoadSceneMode.Additive);
		}

		public static void LoadSceneWithMap(int sceneIndex)
		{
			if (m_isLoading) return;
			m_useMap = true;
			m_useTutorial = false;

			m_isLoading = true;
			m_sceneToLoad = sceneIndex;
			Application.backgroundLoadingPriority = ThreadPriority.High;
			SceneManager.LoadScene(m_loadingSceneIndex, LoadSceneMode.Additive);
		}

		public static void LoadSceneWithTutorial(int sceneIndex)
		{
			if (m_isLoading) return;
			m_useMap = false;
			m_useTutorial = true;

			m_isLoading = true;
			m_sceneToLoad = sceneIndex;
			Application.backgroundLoadingPriority = ThreadPriority.High;
			SceneManager.LoadScene(m_loadingSceneIndex, LoadSceneMode.Additive);
		}

		private void Start()
		{
			StartAsyncOp(m_sceneToLoad);
			if (!m_useMap && !m_useTutorial)
			{
				m_infoDisplay.color = Color.clear;
				StartCoroutine(StartLoadingSimple(m_sceneToLoad));
			}

			if (m_useMap || m_useTutorial)
			{
				StartCoroutine(StartLoadingComplex(m_sceneToLoad));
			}
		}

		private void Init()
		{
			m_tutorial.gameObject.SetActive(m_useTutorial);
			m_map.gameObject.SetActive(m_useMap);

			m_infoDisplay.text = "";
			m_animateableScale.Play();
		}

		private IEnumerator StartLoadingSimple(int sceneIndex)
		{
			FadeIn(m_fadInDuration);
			yield return new WaitForSeconds(m_fadInDuration);
			SetOverlayAlpha(1);

			//unload scene you cumming from
			UnloadActiveScene();

			while (!m_asyncOperation.isDone)
			{
				if (m_asyncOperation.progress >= 0.9f)
				{
					//activate target scene
					m_asyncOperation.allowSceneActivation = true;
					FadeOut(m_fadeOutDuration);
					yield return new WaitForSeconds(m_fadeOutDuration);

					SetOverlayAlpha(0);

					//unload loading scene
					UnloadActiveScene();
				}

				m_isLoading = false;
				yield return null;
			}
		}

		private IEnumerator StartLoadingComplex(int sceneIndex)
		{
			//Fade in to hide scene you cumming from
			FadeIn(m_fadInDuration);
			yield return new WaitForSeconds(m_fadInDuration);
			SetOverlayAlpha(1);

			UnloadActiveScene();

			Init();
			FadeOut(m_fadeOutDuration);
			yield return new WaitForSeconds(m_fadeOutDuration);
			SetOverlayAlpha(0);
			m_infoDisplay.text = "loading...";
			while (!m_asyncOperation.isDone)
			{
				if (m_asyncOperation.progress >= 0.9f)
				{
					m_infoDisplay.text = m_infoText;

					if (Input.GetKeyDown(KeyCode.Mouse0))
					{
						FadeIn(m_fadInDuration);
						yield return new WaitForSeconds(m_fadInDuration);

						m_tutorial.gameObject.SetActive(false);
						m_map.gameObject.SetActive(false);

						m_asyncOperation.allowSceneActivation = true;
						m_infoDisplay.text = "";

						FadeOut(m_fadeOutDuration);
						yield return new WaitForSeconds(m_fadeOutDuration);

						UnloadActiveScene();
					}
				}

				m_isLoading = false;
				yield return null;
			}
		}

		private void StartAsyncOp(int sceneIndex)
		{
			Application.backgroundLoadingPriority = ThreadPriority.High;
			m_asyncOperation = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
			m_asyncOperation.allowSceneActivation = false;
		}

		private void SetOverlayAlpha(float value)
		{
			var color = m_overlay.color;
			color.a = value;
			m_overlay.color = color;
		}

		private void UnloadActiveScene()
		{
			var activeScene = SceneManager.GetActiveScene();
			foreach (var gbj in activeScene.GetRootGameObjects())
			{
				gbj.SetActive(false);
			}

			SceneManager.UnloadSceneAsync(activeScene);
		}

		private void FadeIn(float duration)
		{
			m_overlay.canvasRenderer.SetAlpha(0);
			m_overlay.CrossFadeAlpha(1, duration, true);
		}

		private void FadeOut(float duration)
		{
			m_overlay.CrossFadeAlpha(0, duration, true);
		}
	}
}
#pragma warning restore 0649