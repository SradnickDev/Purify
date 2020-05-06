using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 0649
namespace Units.Enemy.General
{
	public class TargetHighlight : MonoBehaviour
	{
		[SerializeField] private Image m_targetImage;

		[Header("Default Outline")]
		[SerializeField] private Color m_defaultOutline = Color.white;
		[SerializeField, Range(0.0f, 1f)] private float m_outlineSize = 0.1f;

		[Header("Focus Outline")]
		[SerializeField] private Color m_focuscOutline = Color.white;
		[SerializeField, Range(0.0f, 1f)] private float m_focusOutlineSize = 0.1f;

		private readonly int m_showOutline = Shader.PropertyToID("_OutlineSize");
		private readonly int m_outlineColor = Shader.PropertyToID("_OutlineColor");

		private void Start()
		{
			m_targetImage.material = Instantiate(m_targetImage.material);
			Deselect();
		}

		public void Focus()
		{
			m_targetImage.material.SetFloat(m_showOutline, m_focusOutlineSize);
			m_targetImage.material.SetColor(m_outlineColor, m_focuscOutline);
		}

		public void ResetFocus()
		{
			Select();
		}

		public void Select()
		{
			m_targetImage.material.SetFloat(m_showOutline, m_outlineSize);
			m_targetImage.material.SetColor(m_outlineColor, m_defaultOutline);
		}

		public void Deselect()
		{
			m_targetImage.material.SetFloat(m_showOutline, 0);
		}
	}
}
#pragma warning restore 0649