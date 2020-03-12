using System.Collections.Generic;
using Cards.General;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

#pragma warning disable 0649
namespace Misc.PopUp
{
	public class PopUpHandler : MonoBehaviour
	{
		private static PopUpHandler m_instance;

		public static PopUpHandler Instance
		{
			get
			{
				if (m_instance == null)
				{
					m_instance = FindObjectOfType<PopUpHandler>();
				}

				return m_instance;
			}
		}

		[SerializeField] private RectTransform m_rect;
		[SerializeField] private VerticalLayoutGroup m_layoutGroup;
		[SerializeField] private TextPopUp m_textPopUp;
		private List<TextPopUp> m_textPopUps = new List<TextPopUp>();

		private void Awake()
		{
			m_instance = this;
		}

		public void OpenTextPopUp(string header,
								  string txt,
								  RectTransform targetRect,
								  RectAnchor rectAnchor)
		{
			SetTextAnchorBasedOnRectAnchor(rectAnchor);

			var textPopUp = GetPopUp();
			m_rect.transform.position =
				GeneralUtilities.RectPositionBesides(targetRect,
													 m_rect,
													 rectAnchor);
			PreventScreenCutting();
			SetPopUpContent(header, txt, textPopUp);
			ForceLayoutUpdate(textPopUp);
		}

		private void PreventScreenCutting()
		{
			var halfScreenWidth = Screen.currentResolution.width / 2f;
			var halfScreenHeight = Screen.currentResolution.height / 2f;

			var halfRectWidth = m_rect.rect.width / 2f;
			var halfRectHeight = m_rect.rect.height / 2f;

			var left = m_rect.transform.localPosition - new Vector3(halfRectWidth, 0, 0);
			var right = m_rect.transform.localPosition + new Vector3(halfRectWidth, 0, 0);
			var bottom = m_rect.transform.localPosition - new Vector3(0, halfRectHeight, 0);
			var top = m_rect.transform.localPosition + new Vector3(0, halfRectHeight, 0);

			//Left
			if (left.x < -halfScreenWidth)
			{
				m_rect.transform.localPosition =
					new Vector3(-halfScreenWidth + halfRectWidth, m_rect.localPosition.y,
								m_rect.localPosition.z);
			}

			//right
			if (right.x > halfScreenWidth)
			{
				m_rect.transform.localPosition =
					new Vector3(halfScreenWidth - halfRectWidth, m_rect.localPosition.y,
								m_rect.localPosition.z);
			}

			if (bottom.y < -halfScreenHeight)
			{
				m_rect.transform.localPosition =
					new Vector3(m_rect.localPosition.x, -halfScreenHeight + halfRectHeight,
								m_rect.localPosition.z);
			}
		}

		private void SetPopUpContent(string header, string txt, TextPopUp textPopUp)
		{
			textPopUp.Header = header;
		#if UNITY_EDITOR
			if (txt.Contains("" + (char) 13))
			{
				Debug.LogError("Found char/ascii 13, this will break the Description, but im gonna filter it^^!");
			}
		#endif
			textPopUp.Text = txt.Replace("" + (char) 13, "");
		}

		public void OpenTextPopUp(string header, string txt)
		{
			var textPopUp = GetPopUp();
			SetPopUpContent(header, txt, textPopUp);
			textPopUp.gameObject.SetActive(true);
		}

		public void CloseAll()
		{
			m_textPopUps.ForEach(x => x.gameObject.SetActive(false));
		}

		private TextPopUp GetPopUp()
		{
			var textPopUp = m_textPopUps.Find
				(
				 x => x.gameObject.activeInHierarchy == false
				);

			if (textPopUp)
			{
				textPopUp.gameObject.SetActive(true);
			}
			else
			{
				textPopUp = Instantiate(m_textPopUp, m_rect);
				m_textPopUps.Add(textPopUp);
			}

			return textPopUp;
		}

		private void ForceLayoutUpdate(TextPopUp textPopUp)
		{
			LayoutRebuilder.ForceRebuildLayoutImmediate(textPopUp.RectTransform);
			LayoutRebuilder.ForceRebuildLayoutImmediate(m_rect);
		}

		private void SetTextAnchorBasedOnRectAnchor(RectAnchor rectAnchor)
		{
			switch (rectAnchor)
			{
				case RectAnchor.Top:
					m_layoutGroup.childAlignment = TextAnchor.LowerCenter;
					break;
				case RectAnchor.Bottom:
					m_layoutGroup.childAlignment = TextAnchor.UpperCenter;
					break;
				case RectAnchor.Left:
				case RectAnchor.Right:
					m_layoutGroup.childAlignment = TextAnchor.MiddleCenter;
					break;
			}
		}
	}
}
#pragma warning restore 0649