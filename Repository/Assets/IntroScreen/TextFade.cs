using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#pragma warning disable 0649
[RequireComponent(typeof(TextMeshProUGUI))]
public class TextFade : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI m_textMesh;
	[SerializeField] private float m_fadeSpeed = 1.0F;
	[SerializeField] private int m_rolloverCharacterSpread = 10;
	[SerializeField] private Color m_fadeColor;

	private void Start()
	{
		StartCoroutine(AnimateVertexColors());
	}

	private IEnumerator AnimateVertexColors()
	{
		m_textMesh.ForceMeshUpdate();

		var textInfo = m_textMesh.textInfo;
		var currentCharacter = 0;
		var startingCharacterRange = currentCharacter;
		var characterCount = textInfo.characterCount;

		while (startingCharacterRange != characterCount)
		{
			var fadeSteps = (byte) Mathf.Max(1, 255 / m_rolloverCharacterSpread);

			for (var i = startingCharacterRange; i < currentCharacter + 1; i++)
			{
				var materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
				var newVertexColors = textInfo.meshInfo[materialIndex].colors32;
				var vertexIndex = textInfo.characterInfo[i].vertexIndex;
				var alpha =
					(byte) Mathf.Clamp(newVertexColors[vertexIndex + 0].a + fadeSteps, 0, 255);

				// Set new alpha values.
				newVertexColors[vertexIndex + 0].a = alpha;
				newVertexColors[vertexIndex + 1].a = alpha;
				newVertexColors[vertexIndex + 2].a = alpha;
				newVertexColors[vertexIndex + 3].a = alpha;

				newVertexColors[vertexIndex + 0] = newVertexColors[vertexIndex + 0] * m_fadeColor;
				newVertexColors[vertexIndex + 1] = newVertexColors[vertexIndex + 1] * m_fadeColor;
				newVertexColors[vertexIndex + 2] = newVertexColors[vertexIndex + 2] * m_fadeColor;
				newVertexColors[vertexIndex + 3] = newVertexColors[vertexIndex + 3] * m_fadeColor;

				if (alpha == 255)
				{
					startingCharacterRange += 1;
					if (startingCharacterRange == characterCount)
					{
						m_textMesh.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
					}
				}
			}

			m_textMesh.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
			if (currentCharacter + 1 < characterCount) currentCharacter += 1;

			yield return new WaitForSeconds(m_fadeSpeed);
		}

		//smooth out the last bit of color difference
		var time = 0.5f;
		var currentTime = 0f;
		while (currentTime <= time)
		{
			currentTime += Time.deltaTime;
			var t = currentTime / time;
			m_textMesh.color = Color.Lerp(m_textMesh.color, m_fadeColor, t);
		}
	}
}
#pragma warning restore 0649