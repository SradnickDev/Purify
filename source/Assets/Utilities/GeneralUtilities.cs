using Cards.General;
using UnityEngine;

namespace Utilities
{
	public static class GeneralUtilities
	{
		/// <summary>
		/// Calculate Position besides a Rect.
		/// </summary>
		/// <param name="origin">Used to get the World position.</param>
		/// <param name="target">Used to Anchor</param>
		/// <param name="pos">On which side.</param>
		/// <returns>Position besides the Target Rect</returns>
		public static Vector3 RectPositionBesides(RectTransform origin,
												  RectTransform target,
												  RectAnchor pos)
		{
			var originCorners = new Vector3[4];
			origin.GetWorldCorners(originCorners);

			var retVal = new Vector3(0, 0, 0);
			switch (pos)
			{
				case RectAnchor.Top:
					retVal = ((originCorners[1] + originCorners[2]) * 0.5f) +
							 new Vector3(0, target.rect.height / 2f, 0);
					break;
				case RectAnchor.Bottom:
					retVal = ((originCorners[0] + originCorners[3]) * 0.5f) -
							 new Vector3(0, target.rect.height / 2f, 0);
					break;
				case RectAnchor.Left:
					retVal = ((originCorners[0] + originCorners[1]) * 0.5f) -
							 new Vector3(target.rect.width / 2f, 0, 0);
					break;
				case RectAnchor.Right:
					retVal = ((originCorners[2] + originCorners[3]) * 0.5f) +
							 new Vector3(target.rect.width / 2f, 0, 0);
					break;
			}

			return retVal;
		}

		public static Vector3 ScreenCenter => new Vector3(Screen.width / 2f, Screen.height / 2f, 0);

		/// <summary>
		/// Applies Color based on values.
		/// </summary>
		public static string FormatValue(int defaultVal, int newVal, Color less, Color more)
		{
			var retVal = "";
			var color = ColorUtility.ToHtmlStringRGBA(Color.white);
			if (newVal > defaultVal)
			{
				color = ColorUtility.ToHtmlStringRGBA(more);
			}
			else if (newVal < defaultVal)
			{
				color = ColorUtility.ToHtmlStringRGBA(less);
			}

			retVal = $"<color=#{color}>{newVal}</color>";

			return retVal;
		}

		public static Vector3 CalculateQuadraticCurve(float t, Vector2 p0, Vector2 p1, Vector2 p2)
		{
			return CalculateQuadraticCurvePoint(t, p0, p1, p2);
		}

		private static Vector3 CalculateQuadraticCurvePoint(
			float t,
			Vector2 p0,
			Vector2 p1,
			Vector2 p2)
		{
			return (1.0f - t) * (1.0f - t) * p0 +
				   2.0f * (1.0f - t) * t *
				   p1 + t * t * p2;
		}
	}
}