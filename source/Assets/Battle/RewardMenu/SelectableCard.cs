using Cards.General;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utilities;

namespace Battle.RewardMenu
{
	[RequireComponent(typeof(AudioSource))]
	[RequireComponent(typeof(KeywordHandler))]
	public class SelectableCard : MonoBehaviour, ISimplePointer
	{
		public bool IsActive { get; set; } = true;

		private KeywordHandler KeywordHandler
		{
			get
			{
				if (m_keywordHandler == null)
				{
					m_keywordHandler = GetComponent<KeywordHandler>();
				}

				return m_keywordHandler;
			}
		}

		private AudioSource AudioSource
		{
			get
			{
				if (m_audioSource == null)
				{
					m_audioSource = GetComponent<AudioSource>();
				}

				return m_audioSource;
			}
		}

		private KeywordHandler m_keywordHandler;
		private AudioSource m_audioSource;
		private Button m_button;

		private void Awake()
		{
			GetComponentInChildren<CardPlayableEffect>().Reset();
		}

		public void AddListener(UnityAction action)
		{
			m_button = gameObject.AddComponent<Button>();
			m_button.onClick.AddListener(action);
		}

		public void RemoveListener()
		{
			m_button.onClick.RemoveAllListeners();
		}

		public void OnEnter()
		{
			AudioSource.PlayOneShot(AudioSource.clip);
			GetComponentInChildren<CardPlayableEffect>().ForceEnable = true;
			KeywordHandler.EnableKeywords();
		}

		public void OnExit()
		{
			GetComponentInChildren<CardPlayableEffect>().ForceEnable = false;
			KeywordHandler.DisableKeywords();
		}
	}
}