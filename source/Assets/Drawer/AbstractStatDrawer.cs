using Stats.Types;
using UnityEditor;
using UnityEngine;
using Utilities;

namespace Drawer
{
//#####################################################
// Unity doesn't support Property Drawer			  #
// with inheritance / polymorphism					  #
// to solve this just duplicate these lines			  #
// and change the class name and CustomPropertyDrawer #
//#####################################################
#if UNITY_EDITOR
	[CustomPropertyDrawer(typeof(Soul))]
	public class SoulDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			position = EditorGUI.PrefixLabel(position,
											 GUIUtility.GetControlID(FocusType.Passive), label);


			var minProp = property.FindPropertyRelative("m_min");
			var currentProp = property.FindPropertyRelative("m_current");
			var maxProp = property.FindPropertyRelative("m_max");

			var minLabel = new Rect(position.x - 20,
									position.y,
									position.width * 0.2f - 5,
									position.height);

			var minField = new Rect(position.x,
									position.y,
									position.width * 0.2f - 5,
									position.height);


			var currentLabel = new Rect(position.x + position.width * 0.2f,
										position.y,
										position.width * 0.2f - 5,
										position.height);

			var currentField = new Rect(position.x + position.width * 0.4f + 5,
										position.y,
										position.width * 0.2f - 5,
										position.height);

			var maxLabel = new Rect(position.x + position.width * 0.8f - 20,
									position.y,
									position.width * 0.2f - 5,
									position.height);

			var maxField = new Rect(position.x + position.width * 0.8f + 5,
									position.y,
									position.width * 0.2f - 5,
									position.height);

			EditorGUI.LabelField(minLabel, minProp.displayName);
			EditorGUI.LabelField(currentLabel, currentProp.displayName);
			EditorGUI.LabelField(maxLabel, maxProp.displayName);

			EditorGUI.PropertyField(minField, minProp, GUIContent.none);
			EditorGUI.PropertyField(currentField, currentProp, GUIContent.none);
			EditorGUI.PropertyField(maxField, maxProp, GUIContent.none);

			EditorGUI.EndProperty();
		}
	}

	[CustomPropertyDrawer(typeof(Health))]
	public class HealthDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorExtension.TwoValuesField(position, property, label, "m_current", "m_max");
		}
	}

	[CustomPropertyDrawer(typeof(Defense))]
	public class DefenseDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			position = EditorGUI.PrefixLabel(position,
											 GUIUtility.GetControlID(FocusType.Passive), label);

			var minProp = property.FindPropertyRelative("m_current");
			EditorGUI.PropertyField(position, minProp, GUIContent.none);
		}
	}

	[CustomPropertyDrawer(typeof(Energy))]
	public class EnergyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorExtension.TwoValuesField(position, property, label, "m_current", "m_max",
										   "Current", "Base");
		}
	}

	[CustomPropertyDrawer(typeof(Perseverance))]
	public class DexterityDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			position = EditorGUI.PrefixLabel(position,
											 GUIUtility.GetControlID(FocusType.Passive), label);

			var minProp = property.FindPropertyRelative("m_current");
			EditorGUI.PropertyField(position, minProp, GUIContent.none);
		}
	}

	[CustomPropertyDrawer(typeof(Might))]
	public class StrengthDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			position = EditorGUI.PrefixLabel(position,
											 GUIUtility.GetControlID(FocusType.Passive), label);

			var minProp = property.FindPropertyRelative("m_current");
			EditorGUI.PropertyField(position, minProp, GUIContent.none);
		}
	}
#endif
}