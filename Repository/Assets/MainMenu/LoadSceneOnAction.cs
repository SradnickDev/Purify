using LoadingMenu;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#pragma warning disable 0649
[RequireComponent(typeof(Button))]
public class LoadSceneOnAction : MonoBehaviour
{
	[SerializeField] private int m_sceneIndex;
	private Scene m_validScene;
	private Button m_button;

	private void Start()
	{
		m_button = GetComponent<Button>();
		m_button.onClick.AddListener(LoadScene);
	}

	public void LoadScene()
	{
		LoadingScreen.LoadScene(m_sceneIndex);
	}

	private void OnDestroy()
	{
		m_button.onClick.RemoveListener(LoadScene);
	}
}
#pragma warning restore 0649