using System;
using System.Collections.Generic;
using Cards.Effects.General;
using Stats.General;
using Status.General;
using UnityEditor;
using UnityEngine;
using Utilities;

namespace Editors.CardEditor
{
	/// <summary>
	/// Specific elements that are reused at many places.
	/// </summary>
	public static class ElementEditor
	{
	#if UNITY_EDITOR
		private static int m_deleteIdx;
		private static InputAction m_inputAction = new InputAction("Index : ");

		public static void FieldList<T>(List<T> list,
										string name,
										bool drawDefaultStatus,
										bool drawStatusPrimitives,
										bool drawEffect)
		{
			CardPoolEditorStyles.BeginListElement(name);

			for (var i = 0; i < list.Count; i++)
			{
				var effect = list[i];
				EditorGUILayout.BeginVertical(EditorStyles.helpBox);
				EditorGUILayout.LabelField($"{effect.GetType().Name} - [{i}]",
										   EditorStyles.boldLabel);

				Field(effect, EditorStyles.helpBox, drawDefaultStatus, drawStatusPrimitives,
					  drawEffect);

				EditorGUILayout.EndVertical();
			}

			Footer(list);

			CardPoolEditorStyles.EndListElement();
		}

		public static void Field<T>(T source,
									GUIStyle style,
									bool drawDefaultStatus,
									bool drawStatusPrimitives,
									bool drawEffect)
		{
			EditorGUILayout.BeginVertical(style);

			foreach (var fieldInfo in source.GetType().GetFields())
			{
				EditorExtension.CreatePrimitiveFields(fieldInfo, source);

				if (fieldInfo.FieldType == typeof(StatusData) && drawDefaultStatus)
				{
					var val = (StatusData) fieldInfo.GetValue(source);
					var labelName = val == null ? fieldInfo.FieldType.Name : val.GetType().Name;
					EditorExtension.ClassDropDown<StatusData>(new GUIContent(labelName)
															 ,
															  buff =>
																  fieldInfo.SetValue(source, buff));

					if (val != null && drawStatusPrimitives)
					{
						Field(val, style, drawStatusPrimitives, drawDefaultStatus, drawEffect);
					}
				}
				else if (fieldInfo.FieldType == typeof(StatusData) && !drawDefaultStatus)
				{
					var list = Resources.Load<TextAsset>("Status/StatusList");

					if (list)
					{
						var target = PersistentJson.Create<StatusDataList>(list.text).Status;
						var val = (StatusData) fieldInfo.GetValue(source);

						if (val != null)
						{
							//refresh
							var currentStatus = target.Find(x => x.Name == val.Name);
							fieldInfo.SetValue(source, currentStatus);
						}

						var labelName = val == null ? fieldInfo.FieldType.Name : val.Name;
						StatusDropDown(new GUIContent(labelName)
									 , target
									 , buff => fieldInfo.SetValue(source, buff));
					}
				}


				if (fieldInfo.FieldType == typeof(Effect) && drawEffect)
				{
					var val = (Effect) fieldInfo.GetValue(source);
					var labelName = val == null ? fieldInfo.FieldType.Name : val.GetType().Name;
					EditorExtension.ClassDropDown<Effect>(new GUIContent(labelName)
														, buff => fieldInfo.SetValue(source, buff));

					if (val != null)
					{
						Field(val, style, drawStatusPrimitives, drawDefaultStatus, drawEffect);
					}
				}

				if (fieldInfo.FieldType == typeof(AbstractStat))
				{
					var val = (AbstractStat) fieldInfo.GetValue(source);
					var labelName = val == null ? "None" : val.GetType().Name;
					EditorExtension.ClassDropDown<AbstractStat>(new GUIContent(labelName)
															   ,
																stat => fieldInfo
																	.SetValue(source, stat));
				}

				if (fieldInfo.FieldType == typeof(Lens))
				{
					var val = (Lens) fieldInfo.GetValue(source);
					var labelName = val == null ? "None - that's fine" : val.GetType().Name;
					EditorExtension.ClassDropDown<Lens>(new GUIContent(labelName)
													  , lens => fieldInfo.SetValue(source, lens));

					if (val != null)
					{
						Field(val, style, drawStatusPrimitives, drawDefaultStatus, drawEffect);
					}
				}
			}

			EditorGUILayout.EndVertical();
		}

		public static void StatusDropDown(GUIContent guiContent,
										  IEnumerable<StatusData> data,
										  Action<StatusData> action,
										  params GUILayoutOption[] guiLayouts)
		{
			EditorGUILayout.BeginHorizontal();

			EditorGUILayout.PrefixLabel("Status :");

			if (EditorGUILayout.DropdownButton(guiContent, FocusType.Keyboard, guiLayouts))
			{
				var dropdown = new GenericMenu();

				foreach (var status in data)
				{
					dropdown.AddItem(new GUIContent(status.Name), false,
									 () => { action?.Invoke(status); });
				}

				dropdown.ShowAsContext();
			}

			EditorGUILayout.EndHorizontal();
		}

		public static void Footer<T>(IList<T> list)
		{
			EditorGUILayout.BeginVertical(EditorStyles.toolbar);
			EditorGUILayout.BeginHorizontal();

			m_deleteIdx =
				EditorGUILayout.IntField("Index :", m_deleteIdx, EditorStyles.toolbarTextField);

			if (GUILayout.Button("Remove", EditorStyles.toolbarButton))
			{
				if (list.Count > 0 && list.Count >= m_deleteIdx)
				{
					list.RemoveAt(m_deleteIdx);
				}
			}

			EditorExtension.ClassDropDown<T>(new GUIContent("Add Effect"),
											 (list.Add)
										   , EditorStyles.toolbarDropDown);


			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndVertical();
		}
	#endif
	}
}