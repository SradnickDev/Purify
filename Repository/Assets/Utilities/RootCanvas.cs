using System;
using UnityEngine;

namespace Utilities
{
	/// <summary>
	/// Provides current Canvas scale to calculate positioning properly.
	/// Events for Resolution changes.
	/// </summary>
	public class RootCanvas : MonoBehaviour
	{
		public static event Action ResolutionChanged;

		public static Vector3 Scale
		{
			get { return m_scale; }
		}

		private static Vector3 m_scale = new Vector3(0, 0, 0);

		private Vector2 m_previousResolution = new Vector2(0, 0);

		private void Update()
		{
			CheckResolutionForChanges();

			CheckScaleForChanges();
		}

		/// <summary>
		/// Check if scale has been changed.
		/// </summary>
		private void CheckScaleForChanges()
		{
			if (m_scale != transform.localScale)
			{
				m_scale = transform.localScale;
			}
		}

		/// <summary>
		/// Checks if Resolution has been changed.
		/// </summary>
		private void CheckResolutionForChanges()
		{
		#if UNITY_EDITOR
			var editorResolution = GetMainGameViewSize();
			if (editorResolution != m_previousResolution)
			{
				ResolutionChanged?.Invoke();
				m_previousResolution = editorResolution;
			}
		#else
	var currentResolution = Screen.currentResolution;
		if (currentResolution.width != m_previousResolution.x
			&& currentResolution.height != m_previousResolution.y)
		{
			ResolutionChanged?.Invoke();
			m_previousResolution.x = currentResolution.width;
			m_previousResolution.y = currentResolution.height;
		}
		#endif
		}

	#if UNITY_EDITOR
		public static Vector2 GetMainGameViewSize()
		{
			var T = Type.GetType("UnityEditor.GameView,UnityEditor");
			var getSizeOfMainGameView = T.GetMethod("GetSizeOfMainGameView"
												  , System.Reflection.BindingFlags.NonPublic |
													System.Reflection.BindingFlags.Static);
			var res = getSizeOfMainGameView?.Invoke(null, null);
			return (Vector2) res;
		}
	#endif
	}
}