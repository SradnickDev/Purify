using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Battle.General
{
	/// <summary>
	/// Using bezier curve to create Line out of single segments.
	/// These segments are child objects.
	/// </summary>
	public class UILineRenderer : MonoBehaviour
	{
		private readonly List<RectTransform> m_segments = new List<RectTransform>();

		private void Start()
		{
			m_segments.AddRange(Childs());

			Visible(false);
		}

		private IEnumerable<RectTransform> Childs()
		{
			for (var i = 0; i < transform.childCount; i++)
			{
				yield return transform.GetChild(i).GetComponent<RectTransform>();
			}
		}

		public void DrawLine(Vector2 start, Vector2 anchor, Vector2 end)
		{
			CreateLine(start, anchor, end);
		}

		private void CreateLine(Vector2 start, Vector2 anchor, Vector2 end)
		{
			var beginLine = start;
			var endLine = end;

			m_segments[0].position = beginLine;
			m_segments[m_segments.Count - 1].position = endLine;

			m_segments[0].right = LookAt(m_segments[0].position, m_segments[1].position);

			for (var i = 1; i < m_segments.Count - 1; i++)
			{
				var current = m_segments[i];
				var next = m_segments[i + 1];

				var t = i / (m_segments.Count - 1.0f);

				var position = GeneralExtensions.CalculateQuadraticCurve(t,
																		 beginLine,
																		 anchor,
																		 endLine);

				m_segments[i].position = position;

				var dir = LookAt(current.position, next.position);
				current.right = dir;
				next.right = dir;
			}
		}

		public void Visible(bool value)
		{
			foreach (var rectTransform in Childs())
			{
				rectTransform.gameObject.SetActive(value);
			}
		}

		private Vector3 LookAt(Vector2 current, Vector2 next) => (current - next).normalized;
	}
}