using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Utilities
{
	/// <summary>
	/// Extends the Editor with Fields created with Reflection.
	/// </summary>
	public static partial class EditorExtension
	{
	#if UNITY_EDITOR

#region Primitves

		/// <summary>
		/// Creating Primitive Fields using Reflection.
		/// Supports string,float,int,bool,(Unity)Object,Enum,Range.
		/// </summary>
		/// <param name="field">Used to read Meta data about the Field.</param>
		/// <param name="source">Field source</param>
		/// <typeparam name="T">Type</typeparam>
		public static void CreatePrimitiveFields<T>(FieldInfo field, T source)
		{
			if (field.FieldType == typeof(string))
			{
				StringField(field, source);
			}

			if (field.FieldType == typeof(float))
			{
				FloatField(field, source);
			}

			if (field.FieldType == typeof(int))
			{
				IntField(field, source);
			}

			if (field.FieldType == typeof(bool))
			{
				BoolField(field, source);
			}

			if (field.FieldType.BaseType == typeof(Object))
			{
				ObjectField(field, source);
			}

			if (field.FieldType.BaseType == typeof(Enum))
			{
				EnumField(field, source);
			}

			if (field.FieldType == typeof(Range))
			{
				MinMaxField(field, source);
			}
		}

		private static Vector2 ScrollPos = Vector2.zero;

		public static void StringField(FieldInfo field, object source)
		{
			string result;
			if (field.CustomAttributes.Any(attributeData =>
											   attributeData.AttributeType ==
											   typeof(TextAreaAttribute)))
			{
				EditorGUILayout.LabelField(field.Name);
				ScrollPos = EditorGUILayout.BeginScrollView(ScrollPos, GUILayout.Height(50));
				result = EditorGUILayout.TextArea((string) field.GetValue(source),
												  EditorStyles.textArea,
												  GUILayout.ExpandHeight(true));
				EditorGUILayout.EndScrollView();
			}
			else
			{
				result =
					EditorGUILayout.TextField(field.Name, (string) field.GetValue(source));
			}

			field.SetValue(source, result);
		}

		public static void FloatField(FieldInfo field, object source)
		{
			var result = EditorGUILayout.FloatField(field.Name, (float) field.GetValue(source));
			field.SetValue(source, result);
		}

		public static void IntField(FieldInfo field, object source)
		{
			var result = EditorGUILayout.IntField(field.Name, (int) field.GetValue(source));
			field.SetValue(source, result);
		}

		public static void BoolField(FieldInfo field, object source)
		{
			var result = EditorGUILayout.Toggle(field.Name, (bool) field.GetValue(source));
			field.SetValue(source, result);
		}

		public static void EnumField(FieldInfo field, object source)
		{
			var result = EditorGUILayout.EnumPopup(field.Name, (Enum) field.GetValue(source));
			field.SetValue(source, result);
		}

		public static void MinMaxField(FieldInfo field, object source)
		{
			var result = (Range) field.GetValue(source);
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel(field.Name);
			result.Min = EditorGUILayout.IntField("Min", result.Min);
			result.Max = EditorGUILayout.IntField("Max", result.Max);
			field.SetValue(source, result);
			EditorGUILayout.EndHorizontal();
		}

		public static void ObjectField(FieldInfo field, object source)
		{
			var val = (Object) field.GetValue(source);

			field.SetValue(source,
						   EditorGUILayout.ObjectField(field.Name, val, field.FieldType, false));
		}

#endregion

		/// <summary>
		/// Creates a DropDown with a menu of all derived classes from given base class.
		/// </summary>
		/// <param name="guiContent">Label Information.</param>
		/// <param name="onClickedSelection">Executes the event with the selected class.</param>
		/// <param name="guiLayouts">Beautify your Drop Down ;D</param>
		/// <typeparam name="T">Type</typeparam>
		public static void ClassDropDown<T>(GUIContent guiContent,
											Action<T> onClickedSelection,
											params GUILayoutOption[] guiLayouts)
		{
			EditorGUILayout.BeginHorizontal();

			EditorGUILayout.PrefixLabel(typeof(T).Name + ":");

			if (EditorGUILayout.DropdownButton(guiContent, FocusType.Keyboard, guiLayouts))
			{
				var dropdown = new GenericMenu();

				var types = TypeCache.GetTypesDerivedFrom(typeof(T));

				foreach (var type in types)
				{
					dropdown.AddItem(new GUIContent(type.Name), false,
									 () =>
									 {
										 var newType = (T) Activator.CreateInstance(type);
										 onClickedSelection?.Invoke(newType);
									 });
				}

				dropdown.ShowAsContext();
			}

			EditorGUILayout.EndHorizontal();
		}

		/// <summary>
		/// Creates a DropDown with a menu of all derived classes from given base class.
		/// </summary>
		/// <param name="guiContent">Label Information.</param>
		/// <param name="onClickedSelection">Executes the event with the selected class.</param>
		/// <param name="style">Styling Information.</param>
		/// <param name="guiLayouts">Beautify your Drop Down ;D</param>
		/// <typeparam name="T">Type</typeparam>
		public static void ClassDropDown<T>(GUIContent guiContent,
											Action<T> onClickedSelection,
											GUIStyle style,
											params GUILayoutOption[] guiLayouts)
		{
			if (EditorGUILayout.DropdownButton(guiContent, FocusType.Keyboard, style, guiLayouts))
			{
				var dropdown = new GenericMenu();

				var types = TypeCache.GetTypesDerivedFrom(typeof(T));

				foreach (var type in types)
				{
					dropdown.AddItem(new GUIContent(type.Name), false,
									 () =>
									 {
										 var newType = (T) Activator.CreateInstance(type);
										 onClickedSelection?.Invoke(newType);
									 });
				}

				dropdown.ShowAsContext();
			}
		}

		/// <summary>
		/// Single line field with 2 values.
		/// </summary>
		/// <param name="position">rect position</param>
		/// <param name="property">Target property</param>
		/// <param name="label">Label name</param>
		/// <param name="prop1">first target property</param>
		/// <param name="prop2">sec target property</param>
		public static void TwoValuesField(Rect position,
										  SerializedProperty property,
										  GUIContent label,
										  string prop1,
										  string prop2)
		{
			var minProp = property.FindPropertyRelative(prop1);
			var maxProp = property.FindPropertyRelative(prop2);
			TwoValuesField(position, property, label, prop1, prop2, minProp.displayName,
						   maxProp.displayName);
		}

		/// <summary>
		/// Single line field with 2 values.With label naming.
		/// </summary>
		/// <param name="position">rect position</param>
		/// <param name="property">Target property</param>
		/// <param name="label">Label name</param>
		/// <param name="prop1">first target property</param>
		/// <param name="prop2">sec target property</param>
		/// <param name="labelName1">Name from first label</param>
		/// <param name="labelName2">Name from second label</param>
		public static void TwoValuesField(Rect position,
										  SerializedProperty property,
										  GUIContent label,
										  string prop1,
										  string prop2,
										  string labelName1,
										  string labelName2)
		{
			EditorGUI.BeginProperty(position, label, property);

			position = EditorGUI.PrefixLabel(position,
											 GUIUtility.GetControlID(FocusType.Passive), label);


			var minProp = property.FindPropertyRelative(prop1);
			var maxProp = property.FindPropertyRelative(prop2);

			var minLabel = new Rect(position.x - 50,
									position.y,
									position.width * 0.4f - 5,
									position.height);

			var minField = new Rect(position.x,
									position.y,
									position.width * 0.4f - 5,
									position.height);

			var maxLabel = new Rect(position.x + position.width * 0.6f - 30,
									position.y,
									position.width * 0.4f - 5,
									position.height);

			var maxField = new Rect(position.x + position.width * 0.6f + 5,
									position.y,
									position.width * 0.4f - 5,
									position.height);

			EditorGUI.LabelField(minLabel, labelName1);
			EditorGUI.LabelField(maxLabel, labelName2);

			EditorGUI.PropertyField(minField, minProp, GUIContent.none);
			EditorGUI.PropertyField(maxField, maxProp, GUIContent.none);

			EditorGUI.EndProperty();
		}

	#endif
	}
}