using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 0649
namespace Misc {
	public class ApplyShaderPosition : MonoBehaviour
	{
		[SerializeField] private Image m_image;
		private readonly int m_position = Shader.PropertyToID("_Pos");

		private void Start()
		{
			m_image.material = Instantiate(m_image.material);
			m_image.material.SetVector(m_position, m_image.rectTransform.anchoredPosition);
		}

		private void Update()
		{
			SetPosition();
		}

		private void SetPosition()
		{
			m_image.material.SetVector(m_position, m_image.rectTransform.anchoredPosition);
		}
	}
}

#pragma warning restore 0649