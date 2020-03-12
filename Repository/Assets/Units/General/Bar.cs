using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 0649
namespace Units.General
{
	public class Bar : MonoBehaviour
	{
		[SerializeField] private Image m_fillBar;
		[SerializeField] private Image m_delayedFillBar;
		[SerializeField] private Ease m_easeType = Ease.Linear;
		[SerializeField] private float m_easeDuration = 0.25f;
		[SerializeField] private TextMeshProUGUI m_textMesh;

		private string Text
		{
			set
			{
				if (m_textMesh != null)
				{
					m_textMesh.text = value;
				}
			}
		}

		public void SetColor(Color color)
		{
			m_fillBar.color = color;
		}

		public void SetValues(float current, float max)
		{
			m_fillBar.DOFillAmount(current / max, m_easeDuration / 2f)
					 .SetEase(m_easeType)
					 .OnComplete(() =>
					 {
						 m_delayedFillBar.DOFillAmount(current / max, m_easeDuration);
					 });

			Text = $"{current}/{max}";
		}
	}
}
#pragma warning restore 0649