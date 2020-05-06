using Animateables;
using Cards.General;
using Misc.PopUp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

#pragma warning disable 0649
namespace Status
{
	[RequireComponent(typeof(KeywordHandler))] [RequireComponent(typeof(AudioSource))]
	[RequireComponent(typeof(AnimateableScale))]
	public class StatusIcon : MonoBehaviour, ISimplePointer
	{
		public bool IsActive { get; set; } = true;
		[SerializeField] private Image m_icon;
		[SerializeField] private TextMeshProUGUI m_counter;
		[SerializeField] private KeywordHandler m_keywordHandler;
		[SerializeField] private AudioSource m_audioSource;
		[SerializeField] private AnimateableScale m_animateableScale;
		private string m_description;
		private string m_name;

		public string Description
		{
			set
			{
				if (m_keywordHandler)
				{
					m_description = m_keywordHandler.ParseKeywords(value);
				}
			}
		}

		public string Name
		{
			set => m_name = value;
		}

		public void Set(Sprite icon, string value, AudioClip clip)
		{
			m_icon.sprite = icon;
			m_counter.text = value;
			m_animateableScale.Play();
			if (clip && m_audioSource)
			{
				m_audioSource.PlayOneShot(clip);
			}
		}

		public void UpdateDisplay(string value)
		{
			if (m_counter.text != value)
			{
				m_counter.text = value;
				m_animateableScale.Play();
			}
		}

		public void OnEnter()
		{
			PopUpHandler.Instance.OpenTextPopUp(m_name, m_description, m_icon.rectTransform,
												RectAnchor.Bottom);
		}

		public void OnExit()
		{
			PopUpHandler.Instance.CloseAll();
		}
	}
}
#pragma warning restore 0649