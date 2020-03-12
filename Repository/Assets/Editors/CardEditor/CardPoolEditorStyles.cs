using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace Editors.CardEditor
{
	public static class CardPoolEditorStyles
	{
		public static void BeginListElement(string name)
		{
			var style = new GUIStyle("flow node 0");
			style.padding.top -= 20;
			EditorGUILayout.BeginVertical(style);

			var label = new GUIStyle("AssetLabel");
			EditorGUILayout.LabelField(name, label);
		}

		public static void EndListElement()
		{
			EditorGUILayout.EndVertical();
			EditorGUILayout.Space();
		}

		public static void BeginHeaderElement()
		{
			EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.MaxWidth(350));
			EditorGUILayout.Space();
			EditorGUILayout.BeginVertical(GUILayout.MaxWidth(300));
		}

		public static void EndHeaderElement()
		{
			EditorGUILayout.EndVertical();
			EditorGUILayout.Space();
			EditorGUILayout.EndVertical();
		}
	}
}
#endif