using Keyword;
using Misc.PopUp;
using UnityEngine;
using Utilities;

#pragma warning disable 0649
namespace Cards.General
{
	public class KeywordHandler : MonoBehaviour, ISimplePointer
	{
		public bool IsActive { get; set; } = true;

		[SerializeField] private KeywordParser m_keywordParser;
		[SerializeField] private bool m_onPointer;
		[SerializeField] private RectTransform m_targetRect;
		[SerializeField] private RectAnchor m_rectAnchor;

		public void OverrideSettings(RectTransform targetRect, RectAnchor rectAnchor)
		{
			m_targetRect = targetRect;
			m_rectAnchor = rectAnchor;
		}

		public string ParseKeywords(string text)
		{
			var newText = m_keywordParser.Parse(text);
			return newText;
		}

		public void Clear()
		{
			m_keywordParser.Reset();
			ClearPopUps();
		}

		public void Reset()
		{
			m_keywordParser.Reset();
		}

		private void DrawKeywords()
		{
			foreach (var keywords in m_keywordParser.ParsedKeywords)
			{
				var text = keywords.Description;
				var header = keywords.Keyword;
				PopUpHandler.Instance.OpenTextPopUp(header, text, m_targetRect,
													m_rectAnchor);
			}
		}

		private void ClearPopUps()
		{
			PopUpHandler.Instance.CloseAll();
		}

		public void EnableKeywords() => DrawKeywords();

		public void DisableKeywords() => ClearPopUps();

		public void OnEnter()
		{
			if (m_onPointer)
			{
				EnableKeywords();
			}
		}

		public void OnExit()
		{
			if (m_onPointer)
			{
				DisableKeywords();
			}
		}
	}
}
#pragma warning restore 0649