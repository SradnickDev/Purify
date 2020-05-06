using Animateables;
using Cards.General;
using Misc.PopUp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

#pragma warning disable 0649
namespace Units.Enemy.General
{
	[RequireComponent(typeof(Image))]
	[RequireComponent(typeof(KeywordHandler))]
	[RequireComponent(typeof(AnimateableScale))]
	public class IntentionView : MonoBehaviour, ISimplePointer
	{
		[SerializeField] private TextMeshProUGUI m_amountDisplay = null;
		[SerializeField] private KeywordHandler m_keywordHandler;
		[SerializeField] private Image m_image;
		[SerializeField] private AnimateableScale m_animateableScale;
		private string m_description;
		private string m_header;

		public Sprite Icon
		{
			set
			{
				if (m_image)
				{
					m_image.sprite = value;
					m_animateableScale.Play();
				}
			}
		}

		public string AmountInfo
		{
			set
			{
				if (m_amountDisplay)
				{
					m_amountDisplay.text = value;
				}
			}
		}

		public string Description
		{
			set
			{
				if (m_keywordHandler)
				{
					m_keywordHandler.Reset();
					m_description = m_keywordHandler.ParseKeywords(value);
				}
			}
		}

		public string Header
		{
			set => m_header = value;
		}

		public bool IsActive { get; set; } = true;

		public void OnEnter()
		{
			OpenPopup();
		}

		private void OpenPopup()
		{
			//TODO cross call with KeywordHandler
			PopUpHandler.Instance.OpenTextPopUp(m_header, m_description, m_image.rectTransform,
												RectAnchor.Top);
		}

		public void OnExit()
		{
			ClosePopup();
		}

		private void ClosePopup()
		{
			PopUpHandler.Instance.CloseAll();
		}
	}
}
#pragma warning restore 0649