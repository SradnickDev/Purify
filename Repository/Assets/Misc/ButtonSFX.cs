using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#pragma warning disable 0649
namespace Misc
{
	/// <summary>
	/// Hover and Click sound for a button.
	/// </summary>
	[RequireComponent(typeof(Button))]
	[RequireComponent(typeof(AudioSource))]
	public class ButtonSFX : MonoBehaviour, IPointerEnterHandler
	{
		[SerializeField] private AudioClip m_hoverClip;
		[SerializeField] private AudioClip m_clickClip;
		private Button m_button;
		private AudioSource m_audioSource;

		private void Start()
		{
			m_button = GetComponent<Button>();
			m_audioSource = GetComponent<AudioSource>();
			m_button.onClick.AddListener(PlayClickClip);
		}

		private void PlayClickClip() => m_audioSource.PlayOneShot(m_clickClip);
		private void PlayHoverClip() => m_audioSource.PlayOneShot(m_hoverClip);

		public void OnPointerEnter(PointerEventData eventData) => PlayHoverClip();

		private void OnDestroy()
		{
			if (m_button)
			{
				m_button.onClick.RemoveListener(PlayClickClip);
			}
		}
	}
}
#pragma warning restore 0649