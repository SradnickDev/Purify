using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace Animateables
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(AnimateableScale), true)]
	public class AnimateableInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			var animateable = (Animateable) target;
			if (GUILayout.Button("Play(only in PlayMode"))
			{
				if (EditorApplication.isPlaying)
				{
					animateable.Play();
				}
			}
		}
	}
}
#endif