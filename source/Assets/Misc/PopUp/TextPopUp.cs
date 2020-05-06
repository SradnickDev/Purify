using TMPro;
using UnityEngine;

namespace Misc.PopUp {
	public class TextPopUp : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI m_headerDisplay = null;
		[SerializeField] private TextMeshProUGUI m_popUpDisplay = null;
		private RectTransform m_rectTransform;

		public RectTransform RectTransform
		{
			get
			{
				if (!m_rectTransform)
				{
					m_rectTransform = GetComponent<RectTransform>();
				}

				return m_rectTransform;
			}
		}

		public string Header
		{
			set
			{
				if (m_headerDisplay)
				{
					m_headerDisplay.text = value;
				}
			}
		}

		public string Text
		{
			set
			{
				if (m_popUpDisplay)
				{
					m_popUpDisplay.text = value;
				}
			}
		}
	}
}