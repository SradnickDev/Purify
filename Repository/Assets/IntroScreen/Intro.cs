using LoadingMenu;
using UnityEngine;

#pragma warning disable 0649
public class Intro : MonoBehaviour
{
	[SerializeField] private KeyCode m_skipKey;

	private void Update() => SkipToMainMenu();

	private void SkipToMainMenu()
	{
		if (Input.GetKeyDown(m_skipKey))
		{
			LoadingScreen.LoadScene(1);
		}
	}
}

#pragma warning restore 0649