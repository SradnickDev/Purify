#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Keyword
{
	[CustomEditor(typeof(KeywordList))]
	public class KeywordInspector : Editor
	{
		private ReorderableList m_reorderableList;
		private KeywordList m_keywordList;

		public void OnEnable()
		{
			m_keywordList = (KeywordList) target;
			m_reorderableList = new ReorderableList(serializedObject,
													serializedObject.FindProperty("Keywords"),
													false,
													true,
													true,
													true);

			m_reorderableList.drawHeaderCallback = DrawHeaderCallback;
			m_reorderableList.drawElementCallback = DrawElementCallback;
			m_reorderableList.elementHeightCallback = ElementHeightCallback;
		}

		private void DrawHeaderCallback(Rect rect)
		{
			EditorGUI.LabelField(rect, "Keyword List");
		}

		private void DrawElementCallback(Rect rect, int index, bool isactive, bool isfocused)
		{
			var element = m_reorderableList.serializedProperty.GetArrayElementAtIndex(index);
			var keyword = element.FindPropertyRelative("Keyword");
			var description = element.FindPropertyRelative("Description");

			var lineHeight = EditorGUIUtility.singleLineHeight;
			rect.y += 2;
			EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width, lineHeight),
								 $"Tag : [{index}]");
			EditorGUI.PropertyField(new Rect(rect.x, rect.y + lineHeight, rect.width, lineHeight),
									keyword);
			rect.y += lineHeight;
			EditorGUI.PropertyField(new Rect(rect.x, rect.y + lineHeight * 2, rect.width, +lineHeight * 2),
									description);
			element.serializedObject.ApplyModifiedProperties();
		}

		private float ElementHeightCallback(int index)
		{
			return 2 + EditorGUIUtility.singleLineHeight * 6;
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			m_reorderableList.DoLayoutList();
			serializedObject.ApplyModifiedProperties();
			EditorUtility.SetDirty(target);
		}
	}
}
#endif