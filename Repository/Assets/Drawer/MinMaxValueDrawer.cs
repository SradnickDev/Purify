using UnityEditor;
using UnityEngine;
using Utilities;

namespace Drawer
{
#if UNITY_EDITOR
	[CustomPropertyDrawer(typeof(Range))]
	public class MinMaxValueDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorExtension.TwoValuesField(position, property, label, "Min", "Max");
		}
	}
#endif
}