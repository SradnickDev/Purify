using UnityEditor;
using UnityEngine;

namespace Drawer
{
#if UNITY_EDITOR
	[CustomPropertyDrawer(typeof(PreviewSpriteAttribute))]
	public class PreviewSpriteDrawer : PropertyDrawer
	{
		public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
		{
			if (prop.objectReferenceValue != null)
			{
				return _textureSize;
			}
			else
			{
				return base.GetPropertyHeight(prop, label);
			}
		}

		private const float _textureSize = 65;

		public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, prop);

			if (prop.objectReferenceValue != null)
			{
				position.width = EditorGUIUtility.labelWidth;
				GUI.Label(position, prop.displayName);

				position.x += position.width;
				position.width = _textureSize;
				position.height = _textureSize;

				prop.objectReferenceValue =
					EditorGUI.ObjectField(position, prop.objectReferenceValue, typeof(Sprite), false);
			}
			else
			{
				EditorGUI.PropertyField(position, prop, true);
			}

			EditorGUI.EndProperty();
		}
	}
#endif

	public class PreviewSpriteAttribute : PropertyAttribute
	{
		public PreviewSpriteAttribute() { }
	}
}