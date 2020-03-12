using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0649

namespace LoadingMenu.TutorialScreen
{
	public class Tutorial : MonoBehaviour
	{
		[SerializeField] private List<Animator> m_popups;
		[SerializeField] private KeyCode m_forward = KeyCode.Mouse0;
		[SerializeField] private KeyCode m_backward = KeyCode.Mouse1;
		private int m_currentIdx = 0;
		private readonly int m_fadeOut = Animator.StringToHash("FadeOut");
		private readonly int m_fadeIn = Animator.StringToHash("FadeIn");

		private void Start()
		{
			FadeIn();
		}

		private void Update()
		{
			HandleInput();
		}

		private void HandleInput()
		{
			Forward();
			Backward();
		}

		private void Forward()
		{
			if (Input.GetKeyDown(m_forward))
			{
				FadeOut();
				Iterate(1);
				FadeIn();
			}
		}

		private void Backward()
		{
			if (Input.GetKeyDown(m_backward))
			{
				FadeOut();
				Iterate(-1);
				FadeIn();
			}
		}

		/// <summary>
		/// Iterate seamless through a collection with a fixed size, forward and backward.
		/// </summary>
		/// <param name="value"></param>
		private void Iterate(int value)
		{
			m_currentIdx = ((m_currentIdx + value) % m_popups.Count + m_popups.Count) %
						   m_popups.Count;
		}

		private void FadeIn()
		{
			m_popups[m_currentIdx].SetTrigger(m_fadeIn);
		}

		private void FadeOut()
		{
			m_popups[m_currentIdx].SetTrigger(m_fadeOut);
		}
	}
}

#pragma warning restore 0649