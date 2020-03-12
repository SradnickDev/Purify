using Cards.General;
using Misc.PopUp;
using UnityEngine;
using Utilities;

#pragma warning disable 0649
namespace Misc
{
	[RequireComponent(typeof(KeywordHandler))]
	public class HUDToolTip : MonoBehaviour, ISimplePointer
	{
		public bool IsActive { get; set; } = true;
		[SerializeField] private string m_header;
		[SerializeField, TextArea] private string m_text;
		[SerializeField] private RectTransform m_targetRect;
		[SerializeField] private RectAnchor m_rectAnchor;
		[SerializeField] private KeywordHandler KeywordHandler;
		private string m_textToDisplay;

		private void Start()
		{
			KeywordHandler = GetComponent<KeywordHandler>();
			m_textToDisplay = KeywordHandler.ParseKeywords(m_text);
			KeywordHandler.OverrideSettings(m_targetRect, m_rectAnchor);
		}

		public void OnEnter()
		{
			PopUpHandler.Instance.OpenTextPopUp(m_header, m_textToDisplay, m_targetRect,
												m_rectAnchor);
			KeywordHandler.EnableKeywords();
		}

		public void OnExit()
		{
			KeywordHandler.DisableKeywords();
		}
	}
}
#pragma warning restore 0649