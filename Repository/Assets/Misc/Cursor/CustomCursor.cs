using UnityEngine;
using UnityEngine.SceneManagement;

#pragma warning disable 0649
namespace Misc.Cursor
{
	public class CustomCursor : MonoBehaviour
	{
		[SerializeField] private Texture2D m_defaultState;
		[SerializeField] private Texture2D m_clickState;
		[SerializeField] private Vector2 m_hotSpot;
		private Texture2D m_currentState;

		private void Awake()
		{
			var other = FindObjectOfType<CustomCursor>();
			if (other != null && other != this)
			{
				Destroy(other);
			}

			UpdateState(m_defaultState);
			DontDestroyOnLoad(this.gameObject);
		}

		private void OnEnable()
		{
			SceneManager.sceneLoaded += OnSceneLoaded;
		}

		private void OnDisable()
		{
			SceneManager.sceneLoaded -= OnSceneLoaded;
		}

		private void Update() => ManageStateChanges();
		
		private void ManageStateChanges()
		{
			var down = Input.GetMouseButtonDown(0);
			var up = Input.GetMouseButtonUp(0);

			if (down) UpdateState(m_clickState);

			if (up) UpdateState(m_defaultState);
		}

		//Note :  scene changes cause to block main thread, force to reset the state
		private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			UpdateState(m_defaultState);
		}

		private void UpdateState(Texture2D clickState)
		{
			UnityEngine.Cursor.SetCursor(clickState, m_hotSpot, CursorMode.Auto);
		}
	}
}
#pragma warning restore 0649